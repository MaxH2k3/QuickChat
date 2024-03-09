using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;
using WebSignalR.Models;
using WebSignalR.Models.Dto;
using WebSignalR.Repository;
using AutoMapper;

namespace WebSignalR.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public LoginModel(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserDTO UserDTO { get; set; } = null!;
        public string MessageError { get; set; } = null!;
        public async Task<IActionResult> OnPostAsync()
        {
            var userResponse = _userRepository.Login(UserDTO);
            if (userResponse?.Status == HttpStatusCode.OK)
            {
                var user = (User)userResponse.Data!;
                var claims = new List<Claim>
                {
                    new("UserId", user.UserId.ToString()),
                    new("UserName", user.Username!),
                    new("Role", user.Role!),
                    new(ClaimsIdentity.DefaultNameClaimType, user.UserId.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),

                    IsPersistent = true,

                    IssuedUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Redirect("menu");
            }
            else if (userResponse!.Status == HttpStatusCode.BadRequest)
            {
                MessageError = "Wrong password!";
            }
            else if (userResponse.Status == HttpStatusCode.Forbidden)
            {
                MessageError = "Your account is blocked by admin!";
            }
            else
            {
                MessageError = "Server Error!";
            }

            return Redirect("login");
        }

    }
}
