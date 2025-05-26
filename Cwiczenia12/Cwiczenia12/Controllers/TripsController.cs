using Cwiczenia12.DTOs;
using Cwiczenia12.Exceptions;
using Cwiczenia12.Models;
using Cwiczenia12.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController:ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTripsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tripService.GetTripAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip([FromBody] AssignClientToTripDTO assignClientToTripDTO)
    {
        try
        {
            await _tripService.AssignClientToTripAsync(assignClientToTripDTO);
            return Ok("Client assigned to trip successfully");
        }
        catch (ClientAlreadyAssignedException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (TripNotfoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}