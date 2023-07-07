using Lumberjack.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Lumberjack.API.Utilities
{
    public class ApplicationAccessHandler : AuthorizationHandler<ApplicationAccessRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ApplicationAccessHandler> _logger;
        private readonly ILumberjackRepository _lumberjackRepository;

        public ApplicationAccessHandler(IHttpContextAccessor httpContextAccessor,
                                        ILogger<ApplicationAccessHandler> logger,
                                        ILumberjackRepository lumberjackRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lumberjackRepository = lumberjackRepository ?? throw new ArgumentNullException(nameof(lumberjackRepository));
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationAccessRequirement requirement)
        {
            try
            {
                if (await ValidateApplicationAccess(context, _httpContextAccessor.HttpContext.Request))
                {
                    // If the user has access, mark the requirement as succeeded
                    context.Succeed(requirement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while Authorizing user", ex);
            }

            return Task.CompletedTask;
        }

        private async Task<bool> ValidateApplicationAccess(AuthorizationHandlerContext context, HttpRequest httpRequest)
        {
            // Get ApplicationId from Claims
            var applicationIdClaims = GetApplicationId(context.User);

            // Get MessageId from Http Request
            var messageIdHttpRequest = GetMessageId(httpRequest);

            // Perform the logic to check if the user has access to the specified applicationId
            if (httpRequest.Method == "POST" && messageIdHttpRequest == 0)
            {
                // Get ApplicationId from Http Request
                var applicationIdHttpRequest = GetApplicationId(httpRequest);

                // If POST, verify whether ApplicationIds from Claims and RequestBody match
                return (applicationIdClaims == await applicationIdHttpRequest);
            }

            if (applicationIdClaims > 0 && messageIdHttpRequest == 0)
            {
                return true;
            }
            
            return await HasApplicationAccess(messageIdHttpRequest, applicationIdClaims);
        }

        private async Task<bool> HasApplicationAccess(int messageId, int applicationId)
        {
            return await _lumberjackRepository.MessageBelongsToApplicationAsync(messageId, applicationId);
        }

        private int GetMessageId(HttpRequest httpRequest)
        {
            var messageIdValue = httpRequest.RouteValues["messageId"]?.ToString()
                                    ?? httpRequest.Query["messageId"].ToString();
            if (int.TryParse(messageIdValue, out int messageId))
            {
                return messageId;
            }
            return 0;
        }

        private int GetApplicationId(ClaimsPrincipal claimsPrincipal)
        {
            string applicationIdClaims = claimsPrincipal.FindFirstValue("applicationId");

            if (int.TryParse(applicationIdClaims, out int applicationId))
            {
                return applicationId;
            }
            return 0;
        }

        private async Task<int> GetApplicationId(HttpRequest httpRequest)
        {
            if (httpRequest.HasFormContentType)
            { 
                var form = httpRequest.Form;
                var applicationIdForm = form["applicationid"];

                if (int.TryParse(applicationIdForm, out int applicationId))
                {
                    return applicationId;
                }
            }

            // Read the request body
            var requestBody = await ReadRequestBody(httpRequest);
            return DeserializeRequestBody(requestBody);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var requestBody = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                await request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(memoryStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
            }

            // Allow subsequent components to read the request body
            request.Body.Seek(0, SeekOrigin.Begin);

            return requestBody;
        }

        private int DeserializeRequestBody(string requestBody)
        {
            using JsonDocument document = JsonDocument.Parse(requestBody);
            
            JsonElement root = document.RootElement;

            // Access the specific fields you want by using their property names
            if (root.TryGetProperty("applicationId", out JsonElement applicationIdValue))
            {
                return applicationIdValue.GetInt32();
            }
            return 0;
        }
    }
}
