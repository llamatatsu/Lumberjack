using System;
using System.Collections.Generic;

namespace Lumberjack.API.Entities
{
    public partial class User
    {
        public User()
        {
            UserApplicationMaps = new HashSet<UserApplicationMap>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual ICollection<UserApplicationMap> UserApplicationMaps { get; set; }
    }
}
