namespace WebSignalR.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
    }
}
