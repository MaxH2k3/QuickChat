namespace WebSignalR.Models.Dto
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
