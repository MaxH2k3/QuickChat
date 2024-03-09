using Microsoft.EntityFrameworkCore;
using WebSignalR.Models;

namespace WebSignalR.Repository
{
	public class GroupRepository : IGroupRepository
	{
		private readonly WebSignalRContext _context;

		public GroupRepository(WebSignalRContext context)
		{
			_context = context;
		}

		public GroupRepository()
		{
			_context = new WebSignalRContext();
		}

		public async Task<IEnumerable<Group>> GetGroups()
		{
			return await _context.Groups.ToListAsync();
		}
		public async Task<Group?> GetGroup(Guid groupId)
		{
			return await _context.Groups.FirstOrDefaultAsync(g => g.GroupId.Equals(groupId));
		}

		public async Task<bool> IsExisted(string groupName)
		{
			return await _context.Groups.AnyAsync(g => g.GroupName.Equals(groupName));
		}

		public async Task<bool> IsExistedGroupId(string groupId)
		{
			return await _context.Groups.AnyAsync(g => g.GroupId.ToString().Equals(groupId));
		}

		public async Task<Guid> CreateGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName.Trim()) || await IsExisted(groupName))
			{
				return Guid.Empty;
			}

			var group = new Group()
			{
				GroupId = Guid.NewGuid(),
				GroupName = groupName,
				DateCreated = DateTime.UtcNow,
				NumOfMember = 0
			};

			await _context.Groups.AddAsync(group);

			if (await _context.SaveChangesAsync() > 0)
			{
				return group.GroupId;
			}

			return Guid.Empty;
		}

		public async Task<bool> DeleteGroup(Guid groupId)
		{
			var group = await GetGroup(groupId);
			_context.Groups.Remove(group!);

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<Guid> GetIdOfGroup(string groupName)
		{
			return await _context.Groups.Where(g => g.GroupName.Equals(groupName)).Select(g => g.GroupId).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Group>> GetAllGroupWithoutUser(Guid userId)
		{
			var groupUsers = await _context.GroupUsers
				.Include(x => x.Group)
				.Where(gu => gu.UserId.Equals(userId)).ToListAsync();

			var listId = groupUsers.Select(gu => gu.GroupId).ToList();

			return await _context.Groups
					.Where(g => !listId.Contains(g.GroupId))
					.ToListAsync();
		}
	}
}
