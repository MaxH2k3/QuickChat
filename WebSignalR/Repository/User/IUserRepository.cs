using WebSignalR.Models;
using WebSignalR.Models.Dto;

namespace WebSignalR.Repository
{
    public interface IUserRepository
    {
        User? GetUser(string username);
        IEnumerable<User> GetUsers();
        Task<ResponseDTO> Register(RegisterUser registerUser);
        ResponseDTO? Login(UserDTO userDTO);
    }
}
