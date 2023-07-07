using Lumberjack.API.Entities;

namespace Lumberjack.API.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int NumberOfApplications { get { return Applications.Count; } }

        public ICollection<ApplicationDto> Applications { get; set; } = new List<ApplicationDto>();
    }
}
