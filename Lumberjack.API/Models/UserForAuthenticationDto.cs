namespace Lumberjack.API.Models
{
    public class UserForAuthenticationDto
    {
        public string UserName { get; set; } = string.Empty;

        public int ApplicationId { get; set; } = 0;
    }
}
