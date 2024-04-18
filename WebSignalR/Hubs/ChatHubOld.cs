using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Security.Claims;
using WebSignalR.Models;
using WebSignalR.Repository;
using Group = WebSignalR.Models.Group;

namespace WebSignalR.Hubs
{
	public class ChatHubOld : Hub
	{
		// List properties of user;
		private IEnumerable<Claim> Claims => Context.User!.Claims;

		// Repository
		private readonly IUserRepository _userRepository;
		private readonly IGroupRepository _groupRepository;
		private readonly IMessageRepository _messageRepository;
		private readonly IGroupUserRepository _groupUserRepository;

		public ChatHubOld(IUserRepository userRepository, IGroupRepository groupRepository,
			IMessageRepository messageRepository, IGroupUserRepository groupUserRepository)
		{
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_messageRepository = messageRepository;
			_groupUserRepository = groupUserRepository;
		}

		public override async Task OnConnectedAsync()
		{
			var userId = Guid.Parse(Claims.ElementAt(0).Value);

			var groupUsers = await _groupUserRepository.GetAllGroupByUser(userId);

			foreach (var groupUser in groupUsers)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, groupUser.GroupId.ToString());
			}

			await base.OnConnectedAsync();
		}

		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public Task SendPrivateMessage(string user, string message)
		{
			return Clients.User(user).SendAsync("ReceiveMessage", message);
		}

		public async Task AddToGroup(string groupId)
		{
			string userId = Claims.ElementAt(0).Value;

			// Check userId exist on group
			if (!(await _groupUserRepository.IsExistOnGroup(userId.ToString(), groupId.ToString())))
			{
				// Add user to group on database
				await _groupUserRepository.AddUserToGroup(userId, groupId.ToString());

				// Add user to group
				await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());

				// Notification
				await Clients.Group(groupId.ToString()).SendAsync("Send", $"{Claims.ElementAt(1).Value} has joined the group.");

				return;
			}

			Console.WriteLine("You have been joined!");
		}

		public async Task RemoveFromGroup(string groupId)
		{
			string userId = Claims.ElementAt(0).Value;

			// Check userId exist on group
			if (!(await _groupUserRepository.IsExistOnGroup(userId, groupId.ToString())))
			{
				Console.WriteLine("User not found on group!");
				return;
			}

			// Remove user from database
			_ = _groupUserRepository.RemoveUserFromGroup(userId, groupId.ToString());

			// Remove user from group
			await Groups.RemoveFromGroupAsync(userId, groupId.ToString());

			// Notification
			await Clients.Group(groupId).SendAsync("Send", $"{Claims.ElementAt(1).Value} has left the group.");
		}

		public async Task SendMessageToGroup(string groupId, string message)
		{
			// Get userId from cookie
			string userId = Claims.ElementAt(0).Value;

			// Check exist group name	
			if (await _groupUserRepository.IsExistOnGroup(userId, groupId))
			{
				// Add message to database
				var tempMessage = await _messageRepository.AddMessageToGroup(groupId.ToString(), userId, message);

				// Convert to json
				string json = JsonConvert.SerializeObject(tempMessage);

				// Send message
				await Clients.Group(groupId.ToString()).SendAsync("Send", json);

				return;
			}

			Console.WriteLine($"You are not in group");

		}

		public async Task CreateNewGroup(string groupName)
		{
			var groupId = await _groupRepository.CreateGroup(groupName);

			if (groupId == Guid.Empty)
			{
				Console.WriteLine("Group not created!");
				return;
			}

			await Clients.All.SendAsync("DisplayGroup", groupId.ToString(), groupName);
		}

		public async Task ConnectionToAllGroup(IEnumerable<Group> groups)
		{
			foreach (var item in groups)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, item.GroupId.ToString());
			}
		}

		public async Task GetMessageOnGroup(string groupId)
		{
			var messages = await _messageRepository.GetMessageOnGroup(groupId);

			string json = JsonConvert.SerializeObject(messages);

			await Clients.Caller.SendAsync("LoadMessageForGroup", json);
		}

	}
}
