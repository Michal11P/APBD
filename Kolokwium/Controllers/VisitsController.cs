using Kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitsController: Controller
{
    private readonly IVisitsService _visitsService;

    public VisitsController(IVisitsService visitsService)
    {
        _visitsService = visitsService;
    }

    [HttpGet("{visitId}")]
    public async Task<IActionResult> GetVisitHistory(int visitId)
    {
        try
        {
            var result = await _visitsService.GetVisitHistory(visitId);
            return Ok(result);
        }
        catch(Exception e)
        {
            return NotFound();
        }
    }

   // [HttpPost]
   // public Task<IActionResult> AddNewVisit()
   // {
        
   // }
}