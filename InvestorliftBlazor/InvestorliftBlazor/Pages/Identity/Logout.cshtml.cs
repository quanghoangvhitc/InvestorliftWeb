using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestorliftBlazor.Pages.Identity
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync([FromQuery]string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Clear the existing the external login cookie
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch(Exception ex) { string error = ex.Message; }
            return Redirect(returnUrl);
        }
    }
}
