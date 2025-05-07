namespace Kolokwium.Models.DTO;

public class VisitHistoryDTO
{
    public DateTime Date { get; set; }
    public ClientDTO Client { get; set; }
    public MechanicDTO Mechanic { get; set; }
    public List<ServiceDTO> Services { get; set; } = [];
    
}

public class ClientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class MechanicDTO
{
    public int MechanicId { get; set; }
    public string LicenceNumber { get; set; }
}

public class ServiceDTO
{
    public string Name { get; set; }
    public decimal ServiceFee { get; set; }
}