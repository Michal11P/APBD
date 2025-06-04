using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;
[Table("AvailableProgram")]
public class Available_Program
{
    [Key]
    public int AvailableProgramId { get; set; }
    [ForeignKey(nameof(WashingMachine))]
    public int WashingMachineId { get; set; }
    [ForeignKey(nameof(ProgramEntity))]
    public int ProgramId { get; set; }
    [Column(TypeName = "numeric")]
    [Precision(10, 2)]
    public double Price { get; set; }
    
    public Washing_Machine WashingMachine { get; set; }
    public ProgramEntity ProgramEntity { get; set; }

    public ICollection<Purchase_History> Purchase_History { get; set; } = null;

}