using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;

[Table("Purchase_History")]
[PrimaryKey(nameof(AvailableProgramId), nameof(CustomerId))]
public class Purchase_History
{
    [ForeignKey(nameof(Available_Program))]
    public int AvailableProgramId  { get; set; }
    [ForeignKey(nameof(Customer))]
    public int CustomerId  { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int? Rating { get; set; }
}