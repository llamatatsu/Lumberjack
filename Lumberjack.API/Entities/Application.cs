using System;
using System.Collections.Generic;

namespace Lumberjack.API.Entities
{
    public partial class Application
    {
        public Application()
        {
            Messages = new HashSet<Message>();
            UserApplicationMaps = new HashSet<UserApplicationMap>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ReferenceId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<UserApplicationMap> UserApplicationMaps { get; set; }
    }
}
