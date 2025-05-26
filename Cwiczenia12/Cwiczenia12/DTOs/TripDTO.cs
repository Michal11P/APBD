namespace Cwiczenia12.DTOs;

public class TripDTO
{
    public string Name {get; set;}
    public string Descritpion {get; set;}
    public DateTime DateFrom {get; set;}
    public DateTime DateTo {get; set;}
    public int MaxPeople {get; set;}
    public List<CountryDTO> Countries {get; set;}
    public List<ClientDTO> Clients {get; set;}
}

public class ClientDTO
{
    public string FirstName {get; set;}
    public string LastName {get; set;}
}

public class CountryDTO
{
    public string Name {get; set;}
}
public class TripResultDTO
{
    public int PageNumber {get; set;}
    public int PageSize {get; set;}
    public int AllPages {get; set;}
    public List<TripDTO> Trips {get; set;}
}