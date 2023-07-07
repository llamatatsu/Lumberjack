using System.ComponentModel.DataAnnotations;

namespace Lumberjack.API.Models
{
    public class MessageForUpdateDto
    {
        //[Required]
        //public int ApplicationId { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression("Text|File")]
        public string MessageType { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [Required]
        public string ModifiedBy { get; set; }
    }
}
