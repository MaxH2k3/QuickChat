using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebSignalR.Hubs;
using WebSignalR.Models;
using WebSignalR.Repository;

namespace WebSignalR.Pages.Menu
{
	public class MenuModel : PageModel
	{
		private readonly IGroupUserRepository _groupUserRepository;
		private readonly IGroupRepository _groupRepository;
		private readonly IMessageRepository _messageRepository;
		private readonly IMapper _mapper;
		private IEnumerable<Claim> Claims => User!.Claims;

		[BindProperty]
		public IEnumerable<Group> Groups { get; set; } = new List<Group>();

		[BindProperty]
		public IEnumerable<Group> NewGroups { get; set; } = new List<Group>();

		[BindProperty]
		public IEnumerable<Message> Messages { get; set; } = new List<Message>();

		public MenuModel(IGroupUserRepository groupUserRepository,
			IMapper mapper, IGroupRepository groupRepository,
			IMessageRepository messageRepository)
		{
			_groupUserRepository = groupUserRepository;
			_mapper = mapper;
			_groupRepository = groupRepository;
			_messageRepository = messageRepository;
		}

		public async Task<IActionResult> OnGet()
		{
			if (!User.Identity!.IsAuthenticated)
			{
				return Redirect("/login");
			}

			var userId = Guid.Parse(Claims.ElementAt(0).Value);

			// Get all group with user joined
			var groupUsers = await _groupUserRepository.GetAllGroupByUser(userId);
			Groups = _mapper.Map<IEnumerable<Group>>(groupUsers);

			// Get all group without user
			NewGroups = await _groupRepository.GetAllGroupWithoutUser(userId);

			if (Groups.Count() > 0)
			{
				// Get all message of group
				Messages = await _messageRepository.GetMessageOnGroup(Groups.ElementAt(0).GroupId.ToString());
			}

			return Page();
		}
	}
}