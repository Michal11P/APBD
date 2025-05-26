using Cwiczenia12.Data;
using Cwiczenia12.DTOs;
using Cwiczenia12.Exceptions;
using Cwiczenia12.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia12.Services;

public class TripService : ITripService
{
    private readonly ApbdContext _context;

    public TripService(ApbdContext context)
    {
        _context = context;
    }

    public async Task<TripResultDTO> GetTripAsync(int page, int pageSize)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }
        var totalTrips =await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);
        
        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
                .ThenInclude(ct=>ct.IdClientNavigation)
            .Include(t=>t.IdCountries)
            .OrderByDescending(t=>t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t=>new TripDTO
            {
                Name = t.Name,
                Descritpion = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(ct=>new CountryDTO
                {
                    Name = ct.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct=>new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

        return new TripResultDTO
        {
            PageNumber = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        };

    }

    public async Task AssignClientToTripAsync(AssignClientToTripDTO assignClientToTripDTO)
    {
        var trip = await _context.Trips
            .Include(t => t.ClientTrips)
            .FirstOrDefaultAsync(t=>t.IdTrip == assignClientToTripDTO.IdTrip);
        if (trip == null || trip.DateFrom < DateTime.Now)
        {
            throw new TripNotfoundException(assignClientToTripDTO.IdTrip);
        }
        
        
        var client = await _context.Clients
            .Include(c=>c.ClientTrips)
            .FirstOrDefaultAsync(c=>c.Pesel == assignClientToTripDTO.Pesel);
        if (client != null)
        {
            throw new ClientAlreadyAssignedException(assignClientToTripDTO.Pesel, assignClientToTripDTO.IdTrip);
        }
        else
        {
            client = new Client
            {
                FirstName = assignClientToTripDTO.FirstName,
                LastName = assignClientToTripDTO.LastName,
                Email = assignClientToTripDTO.Email,
                Telephone = assignClientToTripDTO.Telephone,
                Pesel = assignClientToTripDTO.Pesel,
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        var newClientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = assignClientToTripDTO.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = assignClientToTripDTO.PaymentDate
        };
        _context.ClientTrips.Add(newClientTrip);
        await _context.SaveChangesAsync();
        
    }
}