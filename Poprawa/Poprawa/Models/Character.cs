using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poprawa.Models;

[Table("Character")]
public class Character
{
    [Key]
    public int CharacterId { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(120)]
    public string LastName { get; set; }
    public int CurrentWeight { get; set; }
    public int MaxWeight { get; set; }
    
    public ICollection<Backpack> Backpacks { get; set; } = new List<Backpack>();
    public ICollection<Character_Title> CharacterTitles { get; set; } = new List<Character_Title>();
}