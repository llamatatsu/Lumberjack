namespace Lumberjack.API.Models
{
    public class MessageWithSegmentsDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; } = 0;
        public string MessageType { get; set; } = string.Empty;
        public int NumberOfSegments { get { return Segments.Count; } }

        public ICollection<SegmentDto> Segments { get; set; } = new List<SegmentDto>();
    }
}
