using Microsoft.AspNetCore.Mvc;
using Poprawa.DTOs;
using Poprawa.Exceptions;
using Poprawa.Services;

namespace Poprawa.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly ICharactersService _charactersService;

    public CharactersController(ICharactersService charactersService)
    {
        _charactersService = charactersService;
    }

    [HttpGet("{characterId}")]
    public async Task<IActionResult> GetCharacters(int characterId)
    {
        try
        {
            var characterInfo = _charactersService.GetCharacterInfo(characterId);
            return Ok(characterInfo);
        }
        catch (CharacterNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception.Message);
        }
    }
    [HttpPost("{characterId}/backpacks")]
    public async Task<IActionResult> AddBackpack(int characterId, [FromBody] AddItemsToBackpackDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _charactersService.AddItemsToBackpack(characterId, dto);
            return Ok();
        }
        catch (CharacterNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (ItemNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (ToMuchWeightException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            return StatusCode(500,"Unexpected error");
        }
    }

}