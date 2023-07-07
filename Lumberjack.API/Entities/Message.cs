using System;
using System.Collections.Generic;

namespace Lumberjack.API.Entities
{
    public partial class Message
    {
        public Message()
        {
            Files = new HashSet<File>();
            Segments = new HashSet<Segment>();
        }

        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string MessageType { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Application Application { get; set; } = null!;
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
    }
}
