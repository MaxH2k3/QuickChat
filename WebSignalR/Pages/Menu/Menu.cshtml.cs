using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebSignalR.Models;
using WebSignalR.Models.Dto;
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
		public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();

		[BindProperty]
		public IEnumerable<GroupDTO> ListGroups { get; set; } = new List<GroupDTO>();

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

			// Get all group
			NewGroups = await _groupRepository.GetAllGroupWithoutUser(userId);

			// Get all group on database
			ListGroups = await _groupRepository.GetGroupsAsync(userId);

			if (Groups.Any())
			{
				// Get all message of group
				Messages = await _messageRepository.GetMessageOnGroup(Groups.ElementAt(0).GroupId.ToString());
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Redirect("login");
		}

		public async Task<JsonResult> OnGetSearchGroup(string name, string userId)
		{
			var groups = await _groupRepository.SearchGroup(name, Guid.Parse(userId));

			return new JsonResult(groups);
		}

	}
}