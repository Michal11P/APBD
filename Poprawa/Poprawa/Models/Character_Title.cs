using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Poprawa.Models;

[Table("Character_Title")]
[PrimaryKey(nameof(CharacterId), nameof(TitleId))]
public class Character_Title
{
    [ForeignKey(nameof(Character))]
    public int CharacterId { get; set; }
    [ForeignKey(nameof(Title))]
    public int TitleId { get; set; }
    public DateTime AcquiredAt { get; set; }

    public Character Character { get; set; } = null!;
    public Title Title { get; set; } = null!;
}