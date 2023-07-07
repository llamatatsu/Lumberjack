namespace Lumberjack.API.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; } = 0;
        public string MessageType { get; set; } = string.Empty;
    }
}
