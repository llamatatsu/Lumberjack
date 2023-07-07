using System;
using System.Collections.Generic;

namespace Lumberjack.API.Entities
{
    public partial class Segment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Level { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; } = null!;
        public string? Additional { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Message Message { get; set; } = null!;
    }
}
