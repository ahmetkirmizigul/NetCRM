using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCRM.Models.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NetCRM.Pages.Customers
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CustomerDTO Customer { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (TempData["Token"] == null)
                return RedirectToPage("/Login");

            TempData.Keep();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TempData["Token"] == null)
                return RedirectToPage("/Login");

            TempData.Keep();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData["Token"]!.ToString());

            var content = new StringContent(JsonSerializer.Serialize(Customer), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:7288/api/customer", content);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Failed to add customer. Please check your input.";
                return Page();
            }

            SuccessMessage = "Customer added successfully!";
            ModelState.Clear();
            return Page();
        }
    }
}
