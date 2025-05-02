using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;



namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientsService)
        {
            _clientService = clientsService;
        }

        //Zwraca liste wycieczek, na ktore zapisany jest konkretny klient
        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            var trips = await _clientService.GetClientTrip(id);
            if (trips == null || !trips.Any())
            {
                return NotFound("Brak wycieczek lub klient nie istnieje."); //404 Not Found
            }

            return Ok(trips); //200 OK
        }

        //Dodaje nowego klienta do bazy danych
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientDTO clientDto)
        {
            // Walidacja modelu (czy wszystkie wymagane dane są poprawne)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //400 Bad Request
            }

            // Wywołanie serwisu aby dodać klienta
            var newClientId = await _clientService.AddClient(clientDto);

            //201 Created + informacje o kliencie
            return CreatedAtAction(nameof(GetClientTrips), new { id = newClientId }, new { id = newClientId });
        }

        //Rejestruje klienta na konkretna wycieczke
        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> RegisterClientToTrip(int id, int tripId)
        {
            var result = await _clientService.RegisterClientToTrip(id, tripId);
            
            //Wykorzystuje Enum RegisterTripResult.cs
            return result switch
            {
                RegisterTripResult.ClientNotFound => NotFound(new{message ="Client not found."}),
                RegisterTripResult.TripNotFound => NotFound(new{message ="Trip not found."}),
                RegisterTripResult.TripFull => BadRequest(new{message ="Trip has reached the maximum number of participants."}),
                RegisterTripResult.AlreadyRegistered => Conflict(new{message = "Client is already registered for this trip."}),
                RegisterTripResult.Success => Ok(new{message = "Client successfully registered for the trip."}),
                _ => StatusCode(500, "Unknown error.") //500 Internal Server Error
            };
        }

        //Usuwa klienta z zapisanej wczesniej wycieczki
        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> UnregisterClientFromTrip(int id, int tripId)
        {
            var result = await _clientService.UnregisterClientFromTrip(id, tripId);
    
            //Wykorzystuje Enum DeleteTripResult.cs
            return result switch
            {
                DeleteTripResult.ClientNotFound => NotFound(new { message = "Client not found." }),
                DeleteTripResult.TripNotFound => NotFound(new { message = "Trip not found." }),
                DeleteTripResult.RegistrationNotFound => NotFound(new { message = "Client is not registered for this trip." }),
                DeleteTripResult.Success => Ok(new { message = "Client successfully unregistered from the trip." }),
                DeleteTripResult.Error => StatusCode(500, new { message = "An unexpected error occurred." }),
                _ => StatusCode(500, new { message = "Unknown error." }) //500 Internal Server Error
            };
        }

    }
}