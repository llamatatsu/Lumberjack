namespace Lumberjack.API.Models
{
    public class SegmentDto
    {
        public int Id { get; set; }        
        public string Level { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;
        public string Text { get; set; } = string.Empty;
        public string? Additional { get; set; }
    }
}
