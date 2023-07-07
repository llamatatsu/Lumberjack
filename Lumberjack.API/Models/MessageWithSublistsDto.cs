namespace Lumberjack.API.Models
{
    public class MessageWithSublistsDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; } = 0;
        public string MessageType { get; set; } = string.Empty;
        public int NumberOfSegments { get { return Segments.Count; } }
        public int NumberOfFiles { get { return Files.Count; } }

        public ICollection<SegmentDto> Segments { get; set; } = new List<SegmentDto>();

        public ICollection<FileDto> Files { get; set; } = new List<FileDto>();
    }
}
