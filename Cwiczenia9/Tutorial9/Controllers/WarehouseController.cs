using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial9.Exceptions;
using Tutorial9.Model;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToWarehouseAsync([FromBody]WarehouseRequestDTO request)
    {
        try
        {
            var newIdProduct = await _warehouseService.AddProductToWarehouseAsync(request);
            return Ok(newIdProduct);
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (WarehouseNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidAmountException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (OrderAlreadyCompletedException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (DataBaseException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Unexpected server error: " + ex.Message });
        }
    }

    [HttpPost("withProcedure")]
    public async Task<IActionResult> AddProductToWarehouseProcedure([FromBody] WarehouseRequestDTO request)
    {
        try
        {
            var newIdProduct = await _warehouseService.AddProductToWarehouseAsync(request);
            return Ok(new {idProduct = newIdProduct});
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (WarehouseNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidAmountException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (OrderAlreadyCompletedException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (DataBaseException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Unexpected server error: " + ex.Message });
        }
        
    }
}