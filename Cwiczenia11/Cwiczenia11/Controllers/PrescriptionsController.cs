using Cwiczenia11.DTOs;
using Cwiczenia11.Exceptions;
using Cwiczenia11.Models;
using Cwiczenia11.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia11.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController:ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescriptionAsync([FromBody] NewPrescription newPrescription)
    {
        try
        {
            await _prescriptionService.AddPrescriptionAsync(newPrescription);
            return StatusCode(201);
        }
        catch (DoctorNotFoundException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (TooManyMedicamentsException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (MedicamentNotFoundException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalitDateRangeException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
        
    }
}