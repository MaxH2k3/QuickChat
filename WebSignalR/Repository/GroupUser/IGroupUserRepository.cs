using WebSignalR.Models;

namespace WebSignalR.Repository
{
    public interface IGroupUserRepository
    {
        Task<IEnumerable<GroupUser>> GetAllGroupByUser(Guid userId);
        Task<bool> IsExistOnGroup(string userId, string groupId);
        Task<bool> AddUserToGroup(string userId, string groupId);
        Task<bool> RemoveUserFromGroup(string userId, string groupId);
    }
}
