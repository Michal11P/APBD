using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium2.Models;
[Table("Customer")]

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    [MaxLength(50)] 
    public string FirstName { get; set; }
    [MaxLength(100)] 
    public string LastName { get; set; }
    [MaxLength(100)]
    public string? PhoneNumber { get; set; }
    
    public ICollection<Purchase_History> Purchase_History { get; set; } = null;
}