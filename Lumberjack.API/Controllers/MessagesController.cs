using AutoMapper;
using Lumberjack.API.Models;
using Lumberjack.API.Services;
using Lumberjack.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Lumberjack.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "ApplicationAccess")]
    [Route("api/Messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMailService _mailService;
        private readonly ILumberjackRepository _lumberjackRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IForkliftServiceBus _forkliftServiceBus;
        private const int maxMessagesPageSize = 20;

        public MessagesController(ILogger<MessagesController> logger,
                                    IMailService mailService,
                                    ILumberjackRepository lumberjackRepository,
                                    IFileService fileService,
                                    IMapper mapper,
                                    IForkliftServiceBus forkliftServiceBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _lumberjackRepository = lumberjackRepository ?? throw new ArgumentNullException(nameof(lumberjackRepository));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _forkliftServiceBus = forkliftServiceBus ?? throw new ArgumentNullException(nameof(forkliftServiceBus));
        }


        /// <summary>
        /// Gets all Messages from the Repository applying filters and searches as they are received
        /// </summary>
        /// <param name="applicationId">The Application ID for the aplication generating the Message</param>
        /// <param name="searchQuery">Applies the searchQuery string to the entire data set</param>
        /// <param name="pageNumber">Specific pageNumber requested</param>
        /// <param name="pageSize">Number of records in a page</param>
        /// <returns>Messages that fit the criteria and Pagination information</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int? applicationId, string? searchQuery,
                                                                                int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // TODO: Remove applicationId from input
                int claimsApplicationId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "applicationId")?.Value);

                pageSize = Math.Min(pageSize, maxMessagesPageSize);

                var (messageEntities, paginationMetadata) = await _lumberjackRepository
                                                                    .GetMessagesAsync(claimsApplicationId, searchQuery,
                                                                                        pageNumber, pageSize);

                Response.Headers.Add("X_Pagination", JsonSerializer.Serialize(paginationMetadata));

                return Ok(_mapper.Map<IEnumerable<MessageDto>>(messageEntities));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting all Messages", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Gets a singular message based on id
        /// </summary>
        /// <param name="messageId">Id of the Message being requested</param>
        /// <param name="includeSegments">Bool that identifies whether the Segment information is required</param>
        /// <returns>Message that is identified by the Id</returns>
        [HttpGet("{messageId}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int messageId, bool includeSegments = false, bool includeFiles = false)
        {
            try
            {
                if (includeSegments && includeFiles)
                {
                    return StatusCode(400, "A message cannot consist of both Segments and Files");
                }

                var message = await _lumberjackRepository.GetMessageAsync(messageId, includeSegments, includeFiles);

                if (message == null)
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                if (includeSegments)
                {
                    return Ok(_mapper.Map<MessageWithSegmentsDto>(message));
                }

                if (includeFiles)
                {
                    return Ok(_mapper.Map<MessageWithFilesDto>(message));
                }

                return Ok(_mapper.Map<MessageDto>(message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Message with id {messageId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Creates a Message with Segments and saves it in the database
        /// </summary>
        /// <param name="message">JSON that holds the Message being created</param>
        /// <returns>The Message with the Id that was created</returns>
        [HttpPost]
        [Route("WithSegments")]
        public async Task<ActionResult> CreateMessageWithSegments([FromBody] MessageWithSegmentsForCreationDto message)
        {
            try
            {
                var finalMessage = _mapper.Map<Entities.Message>(message);

                _lumberjackRepository.AddMessage(finalMessage);

                await _lumberjackRepository.SaveChangesAsync();

                // Erites Messages to the Service Bus
                await _forkliftServiceBus.WriteMessageToServiceBus(finalMessage);

                var createdMessageToReturn = _mapper.Map<MessageWithSegmentsDto>(finalMessage);
                return CreatedAtRoute("GetMessage",
                                        new
                                        {
                                            messageid = createdMessageToReturn.Id
                                        },
                                        createdMessageToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while Creating a Message", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Creates a Message with Files and saves it in the database
        /// </summary>
        /// <param name="message">JSON that holds the Message being created</param>
        /// <returns>The Message with the Id that was created</returns>
        [HttpPost]
        [Route("WithFile")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateMessageWithFile([FromForm] MessageWithFilesForCreationDto message)
        {
            try
            {        
                var (internalFileName, internalFilePath) = await _fileService.StoreFile(message.LogFile);

                message.Files.Add(new FileForCreationDto() 
                { 
                    FileName = message.LogFile.FileName,
                    FileSize = message.LogFile.Length.GenerateFileSize(),
                    InternalFileName = internalFileName,
                    InternalFilePath = internalFilePath
                });

                var finalMessage = _mapper.Map<Entities.Message>(message);

                _lumberjackRepository.AddMessage(finalMessage);

                await _lumberjackRepository.SaveChangesAsync();

                var createdMessageToReturn = _mapper.Map<MessageWithFilesDto>(finalMessage);

                return CreatedAtRoute("GetMessage",
                                        new
                                        {
                                            messageId = createdMessageToReturn.Id
                                        },
                                        createdMessageToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while Creating a Message", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Partially update an existing Message in the database
        /// </summary>
        /// <param name="messageId">Id of the Message to be updated</param>
        /// <param name="patchDocument">JSON Patch document that defines what changes need to happen</param>
        /// <returns>No content</returns>
        [HttpPatch("{messageId}")]
        public async Task<ActionResult> PartiallyUpdateMessage(int messageId,
                                        JsonPatchDocument<MessageForUpdateDto> patchDocument)
        {
            try
            {
                if (!await _lumberjackRepository.MessageExistsAsync(messageId))
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                var messageEntity = await _lumberjackRepository.GetMessageAsync(messageId, false, false);

                var messageToPatch = _mapper.Map<MessageForUpdateDto>(messageEntity);
                messageToPatch.ModifiedDate = DateTime.Now;
                messageToPatch.ModifiedBy = "Lumberjack.API";

                patchDocument.ApplyTo(messageToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Model state for Message with id {messageId} was Invalid");
                    return BadRequest(ModelState);
                }

                if (!TryValidateModel(messageToPatch))
                {
                    _logger.LogInformation($"Model state for Message with id {messageId} was Invalid");
                    return BadRequest(ModelState);
                }

                _mapper.Map(messageToPatch, messageEntity);

                await _lumberjackRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Message with id {messageId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Deletes the Identified Message
        /// </summary>
        /// <param name="messageId">Id of the Message to be deleted</param>
        /// <returns>No Content</returns>
        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            try
            {
                var messageEntity = await _lumberjackRepository.GetMessageAsync(messageId, true, true);

                if (messageEntity == null)
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                _lumberjackRepository.DeleteMessage(messageEntity);

                await _lumberjackRepository.SaveChangesAsync();
                
                _mailService.Send("Message Deleted",
                    $"Message for Application Id {messageEntity.ApplicationId} with Id {messageEntity.Id} was deleted.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while deleting Message with id {messageId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }
    }
}
