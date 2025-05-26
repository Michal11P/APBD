using Cwiczenia12.Exceptions;
using Cwiczenia12.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController:ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{clientId}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId)
    {
        try
        {
            await _clientService.DeteleClientAsync(clientId);
            return Ok($"Client {clientId} deleted successfully");
        }
        catch (NotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ClientHasTripsException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}