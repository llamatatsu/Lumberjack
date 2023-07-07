using System.ComponentModel.DataAnnotations;

namespace Lumberjack.API.Models
{
    public class MessageWithFilesForCreationDto
    {
        [Required]
        public int ApplicationId { get; set; } = 0;

        [Required]
        [MaxLength(10)]
        [RegularExpression("Text|File")]
        public string MessageType { get; set; } = "File";

        [Required]
        public IFormFile LogFile { get; set; }

        [Required]
        public ICollection<FileForCreationDto> Files { get; set; } = new List<FileForCreationDto>();

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; } = "Lumberjack.API";
    }
}
