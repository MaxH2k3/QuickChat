using AutoMapper;
using WebSignalR.Helper;
using WebSignalR.Models;
using WebSignalR.Models.Dto;

namespace WebSignalR.Configuration
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			// Mapping User
			CreateMap<User, UserDTO>().ReverseMap();

			// Maping Group
			CreateMap<GroupUser, Group>()
				.ForMember(dest => dest.GroupName,
						opt => opt.MapFrom(src => src.Group.GroupName));

			// Mapping Message
			CreateMap<Message, MessageDTO>()
				.ForMember(dest => dest.CreatedDate,
					opt => opt.MapFrom(src => TimeHelper.GetTimeSender(src.CreatedDate)));

		}
	}
}
