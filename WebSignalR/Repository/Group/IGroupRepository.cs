using WebSignalR.Models;

namespace WebSignalR.Repository
{
	public interface IGroupRepository
	{
		Task<IEnumerable<Group>> GetGroups();
		Task<bool> IsExisted(string groupName);
		Task<Group?> GetGroup(Guid groupId);
		Task<Guid> CreateGroup(string groupName);
		Task<bool> DeleteGroup(Guid groupId);
		Task<Guid> GetIdOfGroup(string groupName);
		Task<IEnumerable<Group>> GetAllGroupWithoutUser(Guid userId);
		Task<bool> IsExistedGroupId(string groupId);

	}
}
