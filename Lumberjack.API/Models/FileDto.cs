namespace Lumberjack.API.Models
{
    public class FileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public bool IsValid { get; set; } = false;
    }
}
