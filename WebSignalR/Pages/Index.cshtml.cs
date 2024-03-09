using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSignalR.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            if(User.Identity!.IsAuthenticated)
            {
                Response.Redirect("/menu");
            }
            else
            {
                Response.Redirect("/login");
            }
        }
    }
}
