using AutoMapper;
using Lumberjack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Lumberjack.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "ApplicationAccess")]
    [Route("api/Messages/{messageId}/Files")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly ILumberjackRepository _lumberjackRepository;
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        private readonly IMapper _mapper;

        public FilesController(ILogger<FilesController> logger,
                                ILumberjackRepository lumberjackRepository,
                                FileExtensionContentTypeProvider fileExtensionContentTypeProvider,
                                IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lumberjackRepository = lumberjackRepository ?? throw new ArgumentNullException(nameof(lumberjackRepository));
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets a specific File for a Message
        /// </summary>
        /// <param name="messageId">Identifier of the Message</param>
        /// <param name="fileId">Identifier of the File</param>
        /// <returns>File that is identified by the Ids</returns>
        [HttpGet("{fileId}", Name = "GetFile")]
        public async Task<ActionResult> GetFile(int messageId, int fileId)
        {
            try
            {
                if (!await _lumberjackRepository.MessageExistsAsync(messageId))
                {
                    _logger.LogInformation($"Message with id {messageId} wasn't found");
                    return NotFound();
                }

                var file = await _lumberjackRepository.GetFileForMessageAsync(messageId, fileId);

                if (file == null)
                {
                    _logger.LogInformation($"File with id {fileId} wasn't found");
                    return NotFound();
                }

                var storedFile = Path.Combine(file.InternalFilePath, file.InternalFileName);

                if (!System.IO.File.Exists(storedFile))
                {
                    _logger.LogInformation($"File with id {fileId} wasn't found");
                    return NotFound();
                }

                if (!_fileExtensionContentTypeProvider.TryGetContentType(storedFile, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(storedFile);
                return File(bytes, contentType, file.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting Message with id {messageId} or Segment with id {fileId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }
    }
}