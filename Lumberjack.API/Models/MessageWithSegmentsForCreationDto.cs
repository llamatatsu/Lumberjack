using System.ComponentModel.DataAnnotations;

namespace Lumberjack.API.Models
{
    public class MessageWithSegmentsForCreationDto
    {
        [Required]
        public int ApplicationId { get; set; } = 0;

        [Required]
        [MaxLength(10)]
        [RegularExpression("Text|File")]
        public string MessageType { get; set; } = "Text";

        [Required]
        public ICollection<SegmentForCreationDto> Segments { get; set; } = new List<SegmentForCreationDto>();

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; } = "Lumberjack.API";
    }
}
