namespace Poprawa.DTOs;

public class CharacterInfoDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int currentWeight { get; set; }
    public int maxWeight { get; set; }
    public List<BackpackItemsDTO> BackpackItems { get; set; } = new List<BackpackItemsDTO>();
    public List<TitleDTO> Titles { get; set; } = new List<TitleDTO>();
}

public class BackpackItemsDTO
{
    public string itemName { get; set; }
    public int itemWeight { get; set; }
    public int Amount { get; set; }
}

public class TitleDTO
{
    public string title { get; set; }
    public DateTime acquiredAt { get; set; }
}