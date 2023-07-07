using System;
using System.Collections.Generic;

namespace Lumberjack.API.Entities
{
    public partial class UserApplicationMap
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ApplicationId { get; set; }
        public string EncodedToken { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Application Application { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
