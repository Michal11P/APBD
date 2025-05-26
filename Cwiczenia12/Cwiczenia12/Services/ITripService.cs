using Cwiczenia12.DTOs;

namespace Cwiczenia12.Services;

public interface ITripService
{
    Task<TripResultDTO>GetTripAsync(int page, int pageSize);
    Task AssignClientToTripAsync(AssignClientToTripDTO assignClientToTripDTO);
}