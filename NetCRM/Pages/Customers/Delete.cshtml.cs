using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCRM.Models.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NetCRM.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public CustomerDTO Customer { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (TempData["Token"] == null)
                return RedirectToPage("/Login");

            TempData.Keep();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData["Token"]!.ToString());

            var response = await httpClient.GetAsync($"https://localhost:7288/api/customer/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Customer not found.";
                return Page();
            }

            var json = await response.Content.ReadAsStringAsync();
            Customer = JsonSerializer.Deserialize<CustomerDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new CustomerDTO();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TempData["Token"] == null)
                return RedirectToPage("/Login");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData["Token"]!.ToString());

            var response = await httpClient.DeleteAsync($"https://localhost:7288/api/customer/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Failed to delete customer.";
                return Page();
            }

            return RedirectToPage("/Customers/Index");
        }
    }
}
