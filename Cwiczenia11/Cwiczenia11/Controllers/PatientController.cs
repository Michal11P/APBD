using Cwiczenia11.Exceptions;
using Cwiczenia11.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetPatientDetails(int patientId)
    {
        try
        {
            var patient = await _patientService.GetPatientDetails(patientId);
            return Ok(patient);
        }
        catch (PatientNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch(Exception exception)
        {
            return StatusCode(500, exception.Message);
        }
    }
}