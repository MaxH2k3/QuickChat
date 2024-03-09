using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebSignalR.Models;

namespace WebSignalR.Repository
{
    public class GroupUserRepository : IGroupUserRepository
    {
        private readonly WebSignalRContext _context;

        public GroupUserRepository(WebSignalRContext context)
        {
            _context = context;
        }

        public GroupUserRepository()
        {
            _context = new WebSignalRContext();
        }

        public async Task<IEnumerable<GroupUser>> GetAllGroupByUser(Guid userId)
        {
            return await _context.GroupUsers
                .Include(x => x.Group)
                .Where(gu => gu.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<bool> IsExistOnGroup(string userId, string groupId)
        {
            return await _context.GroupUsers.AnyAsync(gu => gu.UserId.ToString().Equals(userId) && gu.GroupId.ToString().Equals(groupId));
        }

        public async Task<bool> AddUserToGroup(string userId, string groupId)
        {
            var groupUser = new GroupUser
            {
                UserId = Guid.Parse(userId),
                GroupId = Guid.Parse(groupId)
            };

            _context.GroupUsers.Add(groupUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveUserFromGroup(string userId, string groupId)
        {
            var groupUser = await _context.GroupUsers.FirstOrDefaultAsync(gu => gu.UserId.ToString().Equals(userId) && gu.GroupId.ToString().Equals(groupId));

            if (groupUser == null)
            {
                return false;
            }

            _context.GroupUsers.Remove(groupUser);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
