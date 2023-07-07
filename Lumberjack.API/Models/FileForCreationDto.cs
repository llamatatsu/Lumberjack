using System.ComponentModel.DataAnnotations;

namespace Lumberjack.API.Models
{
    public class FileForCreationDto
    {
        [Required]
        [MaxLength(2000)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string InternalFilePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string InternalFileName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string FileSize { get; set; } = string.Empty;
        
        [Required]
        public bool IsValid { get; set; } = false;
        
        [Required]        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; } = "Lumberjack.API";
    }
}
