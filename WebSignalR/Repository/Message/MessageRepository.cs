using Microsoft.EntityFrameworkCore;
using WebSignalR.Models;
using WebSignalR.Models.Dto;

namespace WebSignalR.Repository
{
	public class MessageRepository : IMessageRepository
	{
		private readonly WebSignalRContext _context;

		public MessageRepository(WebSignalRContext context)
		{
			_context = context;
		}

		public MessageRepository()
		{
			_context = new WebSignalRContext();
		}

		public async Task<IEnumerable<Message>> GetMessageOnGroup(string groupId)
		{
			return await _context.Messages.Where(g => g.GroupId.ToString().Equals(groupId)).ToListAsync();
		}

		public async Task<Message> AddMessageToGroup(string groupId, string userId, string content)
		{
			var message = new Message()
			{
				UserId = Guid.Parse(userId),
				Content = content,
				GroupId = Guid.Parse(groupId),
				CreatedDate = DateTime.UtcNow
			};
			await _context.Messages.AddAsync(message);

			if (await _context.SaveChangesAsync() > 0)
			{
				return message;
			}

			return null!;
		}

	}
}
