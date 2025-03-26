using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetCRM.Pages;

public class DashboardModel : PageModel
{
    public string Username { get; set; }
    public string Role { get; set; }

    public IActionResult OnGet()
    {
        if (TempData["Token"] == null)
        {
            return RedirectToPage("/Login");
        }

        Username = TempData["Username"]?.ToString() ?? "Kullan�c�";
        Role = TempData["Role"]?.ToString() ?? "Bilinmiyor";

        // TempData'y� koru (bir kez daha kullan�labilsin)
        TempData.Keep();

        return Page();
    }
}
