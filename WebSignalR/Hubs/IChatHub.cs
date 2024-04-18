using WebSignalR.Models;
using WebSignalR.Models.Dto;

namespace WebSignalR.Hubs
{
	public interface IChatHub
	{
		Task SendNotification(string message);
		Task SendMessage(MessageDTO message);
		Task DisplayGroup(string groupId, string groupName);
		Task LoadMessageForGroup(IEnumerable<MessageDTO> messages);
	}
}
