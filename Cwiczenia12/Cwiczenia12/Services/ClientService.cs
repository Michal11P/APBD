using Cwiczenia12.Data;
using Cwiczenia12.Exceptions;
using Cwiczenia12.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia12.Services;

public class ClientService : IClientService
{
    private readonly ApbdContext _context;

    public ClientService(ApbdContext context)
    {
        _context = context;
    }

    public async Task DeteleClientAsync(int clientId)
    {
        var client = await _context.Clients
            .Include(c=>c.ClientTrips)
            .FirstOrDefaultAsync(c=>c.IdClient==clientId);

        if (client == null)
        {
            throw new NotFoundException($"Client with id {clientId} not found");
        }

        if (client.ClientTrips.Any())
        {
            throw new ClientHasTripsException(clientId);
        }
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}