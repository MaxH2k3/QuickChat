using WebSignalR.Models;

namespace WebSignalR.Repository
{
	public interface IMessageRepository
	{
		Task<IEnumerable<Message>> GetMessageOnGroup(string groupId);
		Task<Message> AddMessageToGroup(string groupId, string userId, string content);
	}
}
