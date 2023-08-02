using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using EdgeDB;
using Microsoft.AspNetCore.Identity;

namespace ContactDatabase.Pages;
public class IndexModel : PageModel
{
    private readonly EdgeDBClient _edgeclient;
    public IndexModel(EdgeDBClient client)
    {
        _edgeclient = client;
    }
    [BindProperty]
    public LoginInput LoginInput { get; set; }

    public async Task<IActionResult> OnPost()
    {
        string username = LoginInput.UserName;
        string password = LoginInput.Password;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "username and password fields must be entered");
            return Page();
        }
        var query = @"SELECT Contact {username, password, role } FILTER Contact.username = <str>$username";
        var result = await _edgeclient.QueryAsync<Contact>(query, new Dictionary<string, object?>
        {
           { "username", LoginInput.UserName }
        });
        if(result.Count > 0)
        {
            var passwordHasher = new PasswordHasher<string>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, result.First().Password , LoginInput.Password);
            if (passwordVerificationResult==PasswordVerificationResult.Success)
            {
                    var claims = new List<Claim>
                    {
                       new Claim(ClaimTypes.Name, result.First().Username),
                       new Claim(ClaimTypes.Role, result.First().Role),
                    };
                    var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),

                    };
                    var user = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(scheme, user, authProperties);
                    return RedirectToPage("/ContactList");
            }
            else
            {
               ModelState.AddModelError("", "Invalid password");
               return Page();
            }
        
        }
        else
        {
           ModelState.AddModelError("", "Unsuccessful login attempt ");
           return Page();
            
        }

    }

    public async Task<IActionResult> OnPostLoggingOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Index");
    }
}
public class LoginInput
{
    public string UserName { get; set; }
    public string Password { get; set; }
}