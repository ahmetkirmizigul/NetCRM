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

        Username = TempData["Username"]?.ToString() ?? "Kullanýcý";
        Role = TempData["Role"]?.ToString() ?? "Bilinmiyor";

        // TempData'yý koru (bir kez daha kullanýlabilsin)
        TempData.Keep();

        return Page();
    }
}
