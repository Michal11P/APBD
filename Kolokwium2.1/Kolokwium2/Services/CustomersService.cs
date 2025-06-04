using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Services;

public class CustomersService: ICustomersService
{
    private readonly DatabaseContext _context;

    public CustomersService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<ClientOrderDTO> GetClientOrder(int customerId)
    {
        var order = await _context.Customers
            .Select(c => new ClientOrderDTO
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Orders = c.Purchase_History.Select(p => new OrderDTO
                {
                    PurchaseDate = p.PurchaseDate,
                    Rating = p.Rating,
                    WashingMachine = new WashingMachineDTO
                    {
                        Serial = p.WashingMachine.Serial,
                        MaxWeight = p.WashingMachine.MaxWeight
                    },
                    Program = new ProgramDTO
                    {
                        Name = p.Program.Name,
                        Duration = p.Program.Duration
                    }
                }).ToList()
            }).FirstOrDefaultAsync(c => c.CustomerId == customerId);

        return order ?? new ClientOrderDTO();
    }

}



