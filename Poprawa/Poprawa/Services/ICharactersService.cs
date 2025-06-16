using Poprawa.Data;
using Poprawa.DTOs;

namespace Poprawa.Services;

public interface ICharactersService
{
    Task<CharacterInfoDTO> GetCharacterInfo(int characterId);
    Task AddItemsToBackpack(int characterId, AddItemsToBackpackDTO dTo);
}