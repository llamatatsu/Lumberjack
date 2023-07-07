namespace Lumberjack.API.Models
{
    public class MessageWithFilesDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; } = 0;
        public string MessageType { get; set; } = string.Empty;
        public int NumberOfFiles { get { return Files.Count; } }

        public ICollection<FileDto> Files { get; set; } = new List<FileDto>();
    }
}
