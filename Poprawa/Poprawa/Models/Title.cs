using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poprawa.Models;

[Table("Title")]
public class Title
{
    [Key]
    public int TitleId { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    
    public ICollection<Character_Title> CharacterTitles { get; set; } = new List<Character_Title>();
    
}