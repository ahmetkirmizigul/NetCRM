using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCRM.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace NetCRM.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginRequest LoginRequest { get; set; }

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        using var httpClient = new HttpClient();

        var jsonContent = new StringContent(JsonSerializer.Serialize(LoginRequest), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://localhost:7288/api/auth/login", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Invalid username or password";
            return Page();
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(responseString);

        var token = json.RootElement.GetProperty("token").GetString();
        var username = json.RootElement.GetProperty("username").GetString();
        var role = json.RootElement.GetProperty("role").GetString();

        TempData["Token"] = token;
        TempData["Username"] = username;
        TempData["Role"] = role;

        return RedirectToPage("/Dashboard");
    }
}
