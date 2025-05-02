using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientService
{
    Task<List<ClientTripDTO>> GetClientTrip(int id);
    Task<int> AddClient(ClientDTO client);
    Task<RegisterTripResult> RegisterClientToTrip(int id, int tripId);
    Task<DeleteTripResult> UnregisterClientFromTrip(int id, int tripId);
}