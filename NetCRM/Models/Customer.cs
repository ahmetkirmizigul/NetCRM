using System.ComponentModel.DataAnnotations;

namespace NetCRM.Models;

public class Customer
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MaxLength(100)]
    public string Region { get; set; }

    [Required]
    public DateTime RegistrationDate { get; set; }
}
