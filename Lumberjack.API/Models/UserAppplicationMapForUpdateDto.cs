using System.ComponentModel.DataAnnotations;

namespace Lumberjack.API.Models
{
    public class UserAppplicationMapForUpdateDto
    {
        [Required]
        [MaxLength(2000)]
        public string EncodedToken { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [Required]
        public string ModifiedBy { get; set; }
    }
}
