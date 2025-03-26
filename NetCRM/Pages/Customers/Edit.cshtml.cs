using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCRM.Models.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NetCRM.Pages.Customers
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public CustomerDTO Customer { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

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
                ErrorMessage = "Failed to load customer data.";
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

            TempData.Keep();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempData["Token"]!.ToString());

            var content = new StringContent(JsonSerializer.Serialize(Customer), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"https://localhost:7288/api/customer/{Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Failed to update customer.";
                return Page();
            }

            SuccessMessage = "Customer updated successfully.";
            return Page();
        }
    }
}
