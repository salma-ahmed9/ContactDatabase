using EdgeDB;
using EdgeDB.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

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
            await _edgeclient.ExecuteAsync($$"""
              INSERT Contact{
              first_name := "{{Contact.FirstName}}",
              last_name := "{{Contact.LastName}}",
              email := "{{Contact.Email}}",
              title := "{{Contact.Title}}",
              birth_date := "{{formattedDate}}",
              description := "{{Contact.Description}}",
              marriage_status := {{Contact.MarriageStatus}}

             }
             """);

            return RedirectToPage("/Index");
        }
    }
}
public class Contact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public string BirthDate { get; set; }
    public string Description { get; set; }
    public bool MarriageStatus { get; set; }

}