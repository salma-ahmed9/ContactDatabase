using EdgeDB;
using EdgeDB.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;

namespace ContactDatabase.Pages
{
    public class ContactsModel : PageModel
    {

        private readonly EdgeDBClient _edgeclient;
        public ContactsModel(EdgeDBClient client)
        {
            _edgeclient = client;
        }
        
        [BindProperty]
        public Contact Contact { get; set; }
        public async Task<IActionResult> OnPost()
        {
            if(string.IsNullOrEmpty(Contact.FirstName) || string.IsNullOrEmpty(Contact.LastName) || string.IsNullOrEmpty(Contact.Email) || string.IsNullOrEmpty(Contact.Title) || string.IsNullOrEmpty(Contact.BirthDate)|| string.IsNullOrEmpty(Contact.MarriageStatus.ToString()))
            {
                ModelState.AddModelError("ContactError", "All fields must be filled");
                return Page();
            }
            System.DateTime date = System.DateTime.ParseExact(Contact.BirthDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string formattedDate = date.ToString("dd/MM/yyyy");
            var passwordHasher = new PasswordHasher<string>();
            string hashedPassword = passwordHasher.HashPassword(null,Contact.Password );
            Contact.Password = hashedPassword;
            var query = "INSERT Contact {username := <str>$username, password := <str>$password, role := <str>$role,first_name := <str>$first_name, last_name := <str>$last_name, email := <str>$email, title := <str>$title, birth_date := <str>$birth_date, description := <str>$description, marriage_status := <bool>$marriage_status}";
            await _edgeclient.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"username", Contact.Username},
                {"password", Contact.Password},
                {"role", Contact.Role},
                {"first_name", Contact.FirstName},
                {"last_name", Contact.LastName},
                {"email", Contact.Email},
                {"title", Contact.Title},
                {"birth_date", Contact.BirthDate},
                {"description", Contact.Description},
                {"marriage_status", Contact.MarriageStatus}
            });

            return RedirectToPage("/ContactList");
        }
    }
}
public class Contact
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public string BirthDate { get; set; }
    public string Description { get; set; }
    public bool MarriageStatus { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

}