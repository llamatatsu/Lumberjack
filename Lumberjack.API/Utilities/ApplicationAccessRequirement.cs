using Microsoft.AspNetCore.Authorization;

namespace Lumberjack.API.Utilities
{
    public class ApplicationAccessRequirement: IAuthorizationRequirement
    {
        public string ApplicationId { get; }

        public ApplicationAccessRequirement(string applicationId)
        {
            ApplicationId = applicationId;
        }

        public ApplicationAccessRequirement()
        { 
        
        }
    }
}
