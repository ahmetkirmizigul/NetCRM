using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCRM.DataAccess;
using NetCRM.Models;
using NetCRM.Models.DTOs;

namespace NetCRM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _context.Customers.Select(c => new
        {
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.Region,
            c.RegistrationDate
        }).ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound(new { message = "Müşteri bulunamadı." });

        return Ok(new
        {
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Region,
            customer.RegistrationDate
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Region = dto.Region,
            RegistrationDate = DateTime.SpecifyKind(dto.RegistrationDate, DateTimeKind.Utc)
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Müşteri başarıyla eklendi.", customerId = customer.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO dto)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound(new { message = "Müşteri bulunamadı." });

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.Region = dto.Region;
        customer.RegistrationDate = DateTime.SpecifyKind(dto.RegistrationDate, DateTimeKind.Utc);

        await _context.SaveChangesAsync();

        return Ok(new { message = "Müşteri başarıyla güncellendi." });
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound(new { message = "Müşteri bulunamadı." });

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Müşteri başarıyla silindi." });
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterCustomers([FromQuery] string? name,[FromQuery] string? email,[FromQuery] string? region,[FromQuery] DateTime? startDate,[FromQuery] DateTime? endDate)
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(name.ToLower()) ||
                c.LastName.ToLower().Contains(name.ToLower()));
        }

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(c => c.Email.ToLower().Contains(email.ToLower()));
        }

        if (!string.IsNullOrEmpty(region))
        {
            query = query.Where(c => c.Region.ToLower().Contains(region.ToLower()));
        }

        if (startDate.HasValue)
        {
            query = query.Where(c => c.RegistrationDate >= DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc));
        }

        if (endDate.HasValue)
        {
            query = query.Where(c => c.RegistrationDate <= DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc));
        }

        var result = await query
            .Select(c => new
            {
                c.Id,
                c.FirstName,
                c.LastName,
                c.Email,
                c.Region,
                c.RegistrationDate
            })
            .ToListAsync();

        return Ok(result);
    }


}
