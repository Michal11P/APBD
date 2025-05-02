namespace Tutorial8.Models.DTOs;

public enum RegisterTripResult
{
    Success,
    ClientNotFound,
    TripNotFound,
    TripFull,
    AlreadyRegistered
}