using Kolokwium2.DTOs;

namespace Kolokwium2.Services;

public interface ICustomersService
{
    Task<ClientOrderDTO> GetClientOrder(int customerId);
}