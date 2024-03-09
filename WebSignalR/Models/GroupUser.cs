namespace WebSignalR.Models
{
    public class GroupUser
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual User User { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
    }
}
