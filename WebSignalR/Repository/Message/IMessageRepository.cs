using WebSignalR.Models;
using WebSignalR.Models.Dto;

namespace WebSignalR.Repository
{
	public interface IMessageRepository
	{
		Task<IEnumerable<MessageDTO>> GetMessageOnGroup(string groupId);
		Task<MessageDTO> AddMessageToGroup(string groupId, string userId, string content);
	}
}
