using AutoMapper;
using Lumberjack.API.Entities;
using Lumberjack.API.Models;
using Lumberjack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumberjack.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "ApplicationAccess")]
    [Route("api/Messages/{messageId}/Segments")]
    public class SegmentsController : ControllerBase
    {
        private readonly ILogger<SegmentsController> _logger;
        private readonly ILumberjackRepository _lumberjackRepository;
        private readonly IMapper _mapper;

        public SegmentsController(ILogger<SegmentsController> logger,
                                    ILumberjackRepository lumberjackRepository,
                                    IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lumberjackRepository = lumberjackRepository ?? throw new ArgumentNullException(nameof(lumberjackRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all Segments for a Message
        /// </summary>
        /// <param name="messageId">Identifier of the Message</param>
        /// <returns>List of all Segments being returned</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SegmentDto>>> GetSegments(int messageId)
        {
            try 
            {
                 if (!await _lumberjackRepository.MessageExistsAsync(messageId))
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                var segmentsForMessage = await _lumberjackRepository.GetSegmentsForMessageAsync(messageId);

                return Ok(_mapper.Map<IEnumerable<SegmentDto>>(segmentsForMessage));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Message with id {messageId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Gets a specific Segment for a Message
        /// </summary>
        /// <param name="messageId">Identifier of the Message</param>
        /// <param name="segmentId">Identifier of the Segment</param>
        /// <returns>Segment that is identified by the Ids</returns>
        [HttpGet("{segmentId}", Name = "GetSegment")]
        public async Task<ActionResult<IEnumerable<SegmentDto>>> GetSegment(int messageId, int segmentId)
        {
            try
            {
                if (!await _lumberjackRepository.MessageExistsAsync(messageId))
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                var segment = await _lumberjackRepository.GetSegmentForMessageAsync(messageId, segmentId);

                if (segment == null) 
                {
                    _logger.LogInformation($"Segment with id {segmentId} wasn't found");
                    return NotFound(); 
                }

                return Ok(_mapper.Map<SegmentDto>(segment));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Message with id {messageId} or Segment with id {segmentId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        /// <summary>
        /// Creates a Segment for an existing Message
        /// </summary>
        /// <param name="messageId">Identifier of the Message</param>
        /// <param name="segment">Identifier of the Segment</param>
        /// <returns>Segment information that was created</returns>
        [HttpPost]
        public async Task<ActionResult<SegmentDto>> CreateSegment(int messageId, 
                                            [FromBody] SegmentForCreationDto segment)
        {
            // ApiController attribute ensures that the ModelState is validated
            // Annotations are checked during model binding
            // If ModelState is Invalid, 400 Bad Request is returned with validation errors

            try 
            {
                if (!await _lumberjackRepository.MessageExistsAsync(messageId))
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                if (!await _lumberjackRepository.IsMessageTypeSegmentAsync(messageId))
                {
                    _logger.LogInformation($"Segments cannot be added to Non-Segment type Messages");
                    return Forbid();
                }

                var finalSegment = _mapper.Map<Segment>(segment);

                await _lumberjackRepository.AddSegmentForMessageAsync(messageId , finalSegment);

                await _lumberjackRepository.SaveChangesAsync();

                var createdSegmentToReturn = _mapper.Map<SegmentDto>(finalSegment);

                return CreatedAtRoute("GetSegment",
                                        new
                                        { 
                                            messageId = messageId,
                                            segmentId = createdSegmentToReturn.Id
                                        },
                                        createdSegmentToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Message with id {messageId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }        
    }
}