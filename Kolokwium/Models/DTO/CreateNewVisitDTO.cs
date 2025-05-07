namespace Kolokwium.Models.DTO;

public class CreateNewVisitDTO
{
    public int VisitId { get; set; }
    public int ClientId { get; set; }
    public string LicenceNumber { get; set; }
    public List<ServiceDTO> Services { get; set; } = [];
}