using AutoMapper;
using Lumberjack.API.Entities;
using Lumberjack.API.Models;
using Lumberjack.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lumberjack.API.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly ILumberjackRepository _lumberjackRepository;
        private readonly IMapper _mapper;

        public AuthenticationController(IConfiguration configuration,
                                        ILogger<AuthenticationController> logger,
                                        ILumberjackRepository lumberjackRepository,
                                        IMapper mapper)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lumberjackRepository = lumberjackRepository ?? throw new ArgumentNullException(nameof(lumberjackRepository));
            _mapper = mapper;
        }

        /// <summary>
        /// Authenticates a user and generates a token for the user
        /// </summary>
        /// <param name="userInput">User provided input consisting of a UserName and ApplicationId</param>
        /// <returns>Security Token</returns>
        [HttpPost("Authenticate")]
        public async Task<ActionResult<string>> Authenticate(UserForAuthenticationDto userInput)
        {
            try
            {
                // Step 1: Validate UserName and ApplicationId
                var user = await _lumberjackRepository.ValidateUserAsync(userInput.UserName, userInput.ApplicationId);

                if (user == null)
                {
                    _logger.LogInformation($"User with UserName {userInput.UserName} does not have access on application {userInput.ApplicationId}");
                    return Unauthorized();
                }

                // Step 2: Create a token
                var securityKey = new SymmetricSecurityKey(
                                        Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Claim is a key-value pair of identity related information
                var claimsForToken = new List<Claim>
                                    {
                                        new Claim("sub", user.UserName),
                                        new Claim("given_name", user.FirstName),
                                        new Claim("family_name", user.LastName),
                                        new Claim("applicationId", userInput.ApplicationId.ToString())
                                    };

                var creationDate = DateTime.UtcNow;
                var expiryDate = DateTime.UtcNow.AddHours(1);

                var jwtSecurityToken = new JwtSecurityToken(_configuration["Authentication:Issuer"],
                                                            _configuration["Authentication:Audience"],
                                                            claimsForToken,
                                                            creationDate,
                                                            expiryDate,
                                                            signingCredentials);

                var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                

                // Find if userApplicationMap exists for a user
                var userApplicationMap = await _lumberjackRepository.GetUserApplicationMapAsync(user.Id, userInput.ApplicationId);

                if (userApplicationMap == null)
                {
                    _logger.LogInformation($"UserApplicationMap for UserId {user.Id} and ApplicationID {userInput.ApplicationId} wasn't found");
                    return NotFound();
                }

                // Update Token into UserApplicationMap table
                userApplicationMap.EncodedToken = tokenToReturn;
                userApplicationMap.CreationDate = creationDate;
                userApplicationMap.ExpiryDate = expiryDate;
                userApplicationMap.Active = true;
                userApplicationMap.ModifiedDate = DateTime.Now;
                userApplicationMap.ModifiedBy = "Lumberjack.API";

                await _lumberjackRepository.SaveChangesAsync();

                // Token is only encoded
                // Token can be decoded on jwt.io
                // Token based security (out of the box) relies on https for encrytion

                return Ok(tokenToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while authenticating User with UserName {userInput.UserName}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }
    }
}