using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebSignalR.Models;
using WebSignalR.Models.Dto;

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

		public async Task<IEnumerable<Group>> SearchGroup(string groupName, Guid userId)
		{
			return await _context.GroupUsers
				.Where(g => g.Group.GroupName.Contains(groupName) && g.UserId.Equals(userId))
				.Select(x => x.Group)
				.ToListAsync();
		}

		public async Task<IEnumerable<GroupDTO>> GetGroupsAsync(Guid userId)
		{
			var groups = await (from g in _context.Groups
						 join gu in _context.GroupUsers
						 on g.GroupId equals gu.GroupId into gj
						 from ggu in gj.DefaultIfEmpty()
						 select new GroupDTO
						 {
							 GroupId = g.GroupId,
							 GroupName = g.GroupName,
							 DateCreated = g.DateCreated,
							 NumOfMember = g.NumOfMember,
							 IsJoining = ggu.UserId.Equals(userId)
						 }).ToListAsync();

			/*var groups = await _context.GroupUsers
				.Join(_context.Groups, 
					gu => gu.GroupId,
					g => g.GroupId,
					(gu, g) => new { GroupUser = gu, Group = g })
				.Select(g => new GroupDTO
				{
					GroupId = g.Group.GroupId,
					GroupName = g.Group.GroupName,
					DateCreated = g.Group.DateCreated,
					NumOfMember = g.Group.NumOfMember,
					IsJoining = true
				})
				.ToListAsync();*/

			return groups;
		}
	}
}
