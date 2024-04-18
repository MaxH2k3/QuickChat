using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebSignalR.Helper;
using WebSignalR.Models;
using WebSignalR.Models.Dto;

namespace WebSignalR.Repository
{
	public class MessageRepository : IMessageRepository
	{
		private readonly WebSignalRContext _context;
		private readonly IMapper _mapper;

		public MessageRepository(WebSignalRContext context,
			IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public MessageRepository(IMapper mapper)
		{
			_context = new WebSignalRContext();
			_mapper = mapper;
		}

		public async Task<IEnumerable<MessageDTO>> GetMessageOnGroup(string groupId)
		{
			return await _context.Messages
				.Where(g => g.GroupId.ToString().Equals(groupId))
				.Select(g => new MessageDTO
				{
					MessageId = g.MessageId,
					GroupId = g.GroupId,
					Content = g.Content,
					UserId = g.UserId,
					CreatedDate = TimeHelper.GetTimeSender(g.CreatedDate),
				})
				.ToListAsync();
		}

		public async Task<MessageDTO> AddMessageToGroup(string groupId, string userId, string content)
		{
			var message = new Message()
			{
				UserId = Guid.Parse(userId),
				Content = content,
				GroupId = Guid.Parse(groupId),
				CreatedDate = TimeHelper.GetCurrentInVietName(),
			};
			await _context.Messages.AddAsync(message);

			if (await _context.SaveChangesAsync() > 0)
			{
				return _mapper.Map<MessageDTO>(message);
			}

			return null!;
		}

	}
}
