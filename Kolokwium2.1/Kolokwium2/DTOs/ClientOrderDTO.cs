using Kolokwium2.Models;

namespace Kolokwium2.DTOs;

// public class ClientOrderDTO
// {
//     public string FirstName { get; set; }
//     public string LastName { get; set; }
//     public string? PhoneNumber { get; set; } = null;
//     public List<OrderDTO> Orders { get; set; } = null;
//     
// }
//
// public class OrderDTO
// {
//     public DateTime PurchaseDate{ get; set; }
//     public int? Rating { get; set; }
//     public int Price { get; set; }
//     public WashingMachineDTO WashingMachine { get; set; }
//     public ProgramEntityDTO ProgramEntity { get; set; }
//     
// }
//
//
// public class WashingMachineDTO
// {
//     public string SerialNumber { get; set; }
//     public double MaxWeight { get; set; }
// }
//
// public class ProgramEntityDTO
// {
//     public string Name { get; set; }
//     public int Duration { get; set; }
// }



public class ClientOrderDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public List<OrderDTO> Orders { get; set; }
}

public class OrderDTO
{
    public DateTime PurchaseDate { get; set; }
    public int? Rating { get; set; }
    public double Price { get; set; }
    public WashingMachineDTO WashingMachine { get; set; }
    public ProgramDTO Program { get; set; }
}

public class WashingMachineDTO
{
    public string Serial { get; set; }
    public double MaxWeight { get; set; }
}

public class ProgramDTO
{
    public string Name { get; set; }
    public int Duration { get; set; }
}
