using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebSignalR.Models.Dto;
using WebSignalR.Repository;

namespace WebSignalR.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public RegisterModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public string MessageError { get; set; } = null!;
        public async Task<IActionResult> OnPostAsync(RegisterUser registerUser)
        {
            var responseDTO = await _userRepository.Register(registerUser);
            if (responseDTO.Status == HttpStatusCode.Created)
            {
                return RedirectToPage("Login");
            }
            MessageError = responseDTO.Message!;
            return Page();
        }

    }
}
