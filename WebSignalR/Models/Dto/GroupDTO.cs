namespace WebSignalR.Models.Dto
{
	public class GroupDTO
	{
		public Guid GroupId { get; set; }
		public string GroupName { get; set; } = null!;
		public int NumOfMember { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public bool IsJoining { get; set; }
	}
}
