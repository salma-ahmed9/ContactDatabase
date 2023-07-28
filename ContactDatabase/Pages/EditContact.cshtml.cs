using EdgeDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactDatabase.Pages;
public class EditContactModel : PageModel
{
    private readonly EdgeDBClient _edgeclient;
    public EditContactModel(EdgeDBClient client)
    {
        _edgeclient = client;
    }
    [BindProperty]
    public Contact EditingContact { get; set; } = new Contact();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        EditingContact = await _edgeclient.QuerySingleAsync<Contact>("SELECT Contact{*} FILTER .id = <uuid>$id", new Dictionary<string, object?> { { "id", id } });
        return Page();
    }

    public async Task<IActionResult> OnPostEditContact()
    {
        Guid id = Guid.Parse(Request.Form["id"]);
        EditingContact.Id = id;
        if (string.IsNullOrEmpty(EditingContact.FirstName) || string.IsNullOrEmpty(EditingContact.LastName) || string.IsNullOrEmpty(EditingContact.Email) || string.IsNullOrEmpty(EditingContact.Title) || string.IsNullOrEmpty(EditingContact.BirthDate) || string.IsNullOrEmpty(EditingContact.Username) || string.IsNullOrEmpty(EditingContact.Password))
        {
            ModelState.AddModelError("ContactError", "All fields must be filled");
            return Page();
        }
        System.DateTime date = System.DateTime.ParseExact(EditingContact.BirthDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        string formattedDate = date.ToString("dd/MM/yyyy");
        var passwordHasher = new PasswordHasher<string>();
        string hashedPassword = passwordHasher.HashPassword(null, EditingContact.Password);
        EditingContact.Password = hashedPassword;
        var query = "Update Contact FILTER .id = <uuid>$id SET {username := <str>$username, password := <str>$password, role := <str>$role,first_name := <str>$first_name, last_name := <str>$last_name, email := <str>$email, title := <str>$title, birth_date := <str>$birth_date, description := <str>$description, marriage_status := <bool>$marriage_status} ";
        await _edgeclient.ExecuteAsync(query, new Dictionary<string, object?>
        {
            {"id", EditingContact.Id},
            {"username", EditingContact.Username},
            {"password", EditingContact.Password},
            {"role", EditingContact.Role},
            {"first_name", EditingContact.FirstName},
            {"last_name", EditingContact.LastName},
            {"email", EditingContact.Email},
            {"title", EditingContact.Title},
            {"birth_date", EditingContact.BirthDate},
            {"description", EditingContact.Description},
            {"marriage_status", EditingContact.MarriageStatus}
        });
        return RedirectToPage("/ContactList");
    }
}
        
        
    

