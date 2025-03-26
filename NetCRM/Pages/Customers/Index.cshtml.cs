using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCRM.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NetCRM.Pages.Customers;

public class IndexModel : PageModel
{
    public List<Customer> Customers { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string? name, string? email, string? region, DateTime? startDate, DateTime? endDate)
    {
        if (TempData["Token"] == null)
            return RedirectToPage("/Login");

        TempData.Keep();
        var token = TempData["Token"].ToString();

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var query = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(name)) query.Add("name", name);
        if (!string.IsNullOrWhiteSpace(email)) query.Add("email", email);
        if (!string.IsNullOrWhiteSpace(region)) query.Add("region", region);
        if (startDate.HasValue) query.Add("startDate", startDate.Value.ToString("yyyy-MM-dd"));
        if (endDate.HasValue) query.Add("endDate", endDate.Value.ToString("yyyy-MM-dd"));

        var queryString = query.Count > 0
            ? "?" + string.Join("&", query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"))
            : "";

        var response = await httpClient.GetAsync($"https://localhost:7288/api/customer/filter{queryString}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Failed to load customer list.";
            return RedirectToPage("/Dashboard");
        }

        var content = await response.Content.ReadAsStringAsync();
        Customers = JsonSerializer.Deserialize<List<Customer>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Customer>();

        return Page();
    }

}
