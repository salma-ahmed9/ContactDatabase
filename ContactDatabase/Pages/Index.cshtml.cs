using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EdgeDB;


namespace ContactDatabase.Pages;
[BindProperties]
public class IndexModel : PageModel
{
    private readonly EdgeDBClient _edgeclient;
    public List<Contact> ContactList { get; set; } = new();
    public List<Contact> NewContactList { get; set; } = new();
    public IndexModel(EdgeDBClient client)
    {
        _edgeclient = client;
    }
    public async Task<IActionResult> OnGet()
    {
        var result = await _edgeclient.QueryAsync<Contact>("SELECT Contact {first_name,last_name,email,title,birth_date,description,marriage_status};");
        ContactList=result.ToList();
        return Page();
    }

    public async Task<IActionResult> OnGetSearch(string searchValue)
    {
        // Response.ContentType = "application/json";
        var output = await _edgeclient.QueryAsync<Contact>("SELECT Contact {first_name,last_name,email,title,birth_date,description,marriage_status};");
        ContactList = output.ToList();

        if (string.IsNullOrEmpty(searchValue))
        {
            var data = new { list = ContactList };
            return new JsonResult(data);
        }
        searchValue = searchValue.ToLower();
        foreach(var contact in ContactList)
        {
            if(contact.FirstName.ToLower().Contains(searchValue) || contact.LastName.ToLower().Contains(searchValue) || contact.Email.ToLower().Contains(searchValue))
            {
                NewContactList.Add(contact);
            }
        }
        var result= new { list = NewContactList };
        return new JsonResult(result);
    }



}
