using System.ComponentModel.DataAnnotations;

namespace Lumberjack.API.Models
{
    public class SegmentForCreationDto
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("Info|Debug|Warning|Error|Fatal")]
        public string Level { get; set; } = string.Empty;

        [Required]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string? Additional { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; } = "Lumberjack.API";
    }
}
