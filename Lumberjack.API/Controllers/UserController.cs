using AutoMapper;
using Lumberjack.API.Models;
using Lumberjack.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lumberjack.API.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ILumberjackRepository _lumberjackRepository;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger,
                                ILumberjackRepository lumberjackRepository,
                                IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lumberjackRepository = lumberjackRepository ?? throw new ArgumentNullException(nameof(lumberjackRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns User information for a username
        /// </summary>
        /// <param name="userName">Username to return information for</param>
        /// <returns>User data</returns>
        [HttpGet("{userName}")]
        public async Task<ActionResult<UserDto>> GetUser(string userName)
        {
            try
            {
                var (user, applications) = await _lumberjackRepository.GetUserInfoForUsernameAsync(userName);

                if (user == null)
                {
                    _logger.LogInformation($"User with username {userName} wasn't found");
                    return NotFound();
                }

                var userToReturn = _mapper.Map<UserDto>(user);

                var apps = _mapper.Map<IEnumerable<ApplicationDto>>(applications);

                userToReturn.Applications = (ICollection<ApplicationDto>)apps;

                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting user with username {userName}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }
    }
}
