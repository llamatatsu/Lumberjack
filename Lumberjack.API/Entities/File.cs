using System;
using System.Collections.Generic;

namespace Lumberjack.API.Entities
{
    public partial class File
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; } = null!;
        public string InternalFilePath { get; set; } = null!;
        public string InternalFileName { get; set; } = null!;
        public string FileSize { get; set; } = null!;
        public bool IsValid { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Message Message { get; set; } = null!;
    }
}
