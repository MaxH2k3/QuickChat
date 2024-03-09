using System.Data;
using System.Net;
using WebSignalR.Models.Dto;
using WebSignalR.Models;
using AutoMapper;
using WebSignalR.Repository;

namespace WebSignalR.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WebSignalRContext _context;
        private readonly IAuthentication _authentication;

        public UserRepository(WebSignalRContext context, IAuthentication authentication)
        {
            _context = context;
            _authentication = authentication;
        }

        public UserRepository(IAuthentication authentication)
        {
            _context = new WebSignalRContext();
            _authentication = authentication;
        }

        public User? GetUser(string username)
        {
            return GetUsers().FirstOrDefault(o => o.Username!.Equals(username) || o.Email!.Equals(username));
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public async Task<ResponseDTO> Register(RegisterUser registerUser)
        {
            if (isExisted("username", registerUser.Username))
            {
                return new ResponseDTO(HttpStatusCode.Conflict, "UserName have been existed!");
            }
            else if (isExisted("email", registerUser.Email))
            {
                return new ResponseDTO(HttpStatusCode.Conflict, "Email have been existed!");
            }

            _authentication.CreatePasswordHash(registerUser.Password!, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                UserId = Guid.NewGuid(),
                Email = registerUser.Email,
                Username = registerUser.Username,
                DateCreated = DateTime.UtcNow,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Role = registerUser.Role,
                Status = "Active"
            };

            await _context.Users.AddAsync(user);

            if (await _context.SaveChangesAsync() > 0)
            {
                return new ResponseDTO(HttpStatusCode.Created, "Register successfully!", user.UserId);
            }

            return new ResponseDTO(HttpStatusCode.ServiceUnavailable, "Fail to register!");

        }

        public bool isExisted(string? field, string? value)
        {
            if (field!.ToLower().Equals("username"))
            {
                return GetUsers().Any(u => u.Username!.ToLower().Equals(value!.ToLower()));
            }
            else if (field.ToLower().Equals("email"))
            {
                return GetUsers().Any(u => u.Email!.ToLower().Equals(value!.ToLower()));
            }
            return false;

        }

        public ResponseDTO? Login(UserDTO userDTO)
        {
            var user = GetUser(userDTO.UserName!);
            if (user == null)
            {
                return new ResponseDTO(HttpStatusCode.NotFound, "UserName or Email not existed!");
            }
            if (!_authentication.VerifyPasswordHash(userDTO.Password!, user.Password!, user.PasswordSalt!))
            {
                return new ResponseDTO(HttpStatusCode.BadRequest, "Wrong password!");
            }
            if (user.Status!.Equals("Block"))
            {
                return new ResponseDTO(HttpStatusCode.Forbidden, "Your account is blocked by admin!");
            }
            return new ResponseDTO(HttpStatusCode.OK, "Login successfully!", user);
        }
    }
}
