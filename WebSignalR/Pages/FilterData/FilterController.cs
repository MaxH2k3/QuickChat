using Microsoft.AspNetCore.Mvc;
using WebSignalR.Models;
using WebSignalR.Repository;

namespace WebSignalR.Pages.FilterData
{
	public class FilterController : Controller
	{
		private readonly IGroupRepository _groupRepository;

		public FilterController(IGroupRepository groupRepository)
		{
			_groupRepository = groupRepository;
		}

		public async Task<IEnumerable<Group>> SearchGroup(string name, string userId)
		{
			var groups = await _groupRepository.SearchGroup(name, Guid.Parse(userId));

			return groups;
		}
	}
}
