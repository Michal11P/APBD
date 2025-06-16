using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Poprawa.Models;

[Table("Backpack")]
[PrimaryKey(nameof(ItemId), nameof(CharacterId))]
public class Backpack
{
    [ForeignKey(nameof(Character))]
    public int CharacterId { get; set; }
    [ForeignKey(nameof(Item))]
    public int ItemId { get; set; }
    public int Amount { get; set; }

    public Character Character { get; set; } = null!;
    public Item Item { get; set; } = null!;
}