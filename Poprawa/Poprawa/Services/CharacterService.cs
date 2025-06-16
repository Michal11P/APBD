using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poprawa.Data;
using Poprawa.DTOs;
using Poprawa.Exceptions;
using Poprawa.Models;

namespace Poprawa.Services;

public class CharacterService:ICharactersService
{
    private readonly DatabaseContext _context;

    public CharacterService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<CharacterInfoDTO> GetCharacterInfo(int characterId)
    {
        var characterExists = await _context.Characters.FirstOrDefaultAsync(c=>c.CharacterId == characterId);
        if (characterExists == null)
        {
            throw new CharacterNotFoundException(characterId);
        }
        
        var characterInfo = await _context.Characters
            .Include(c => c.Backpacks).ThenInclude(b => b.Item)
            .Include(c => c.CharacterTitles).ThenInclude(ct => ct.Title)
            .Where(c => c.CharacterId == characterId)
            .Select(c => new CharacterInfoDTO
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                currentWeight = c.CurrentWeight,
                maxWeight = c.MaxWeight,
                BackpackItems = c.Backpacks.Select(bi => new BackpackItemsDTO()
                {
                    itemName = bi.Item.Name,
                    itemWeight = bi.Item.Weight,
                    Amount = bi.Amount
                }).ToList(),
                Titles = c.CharacterTitles.Select(t => new TitleDTO()
                {
                    title = t.Title.Name,
                    acquiredAt = t.AcquiredAt
                }).ToList()

            }).FirstOrDefaultAsync();
        
        return characterInfo;
        
    }

    public async Task AddItemsToBackpack(int characterId, AddItemsToBackpackDTO dto)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            if (dto.ItemIds == null || dto.ItemIds.Any())
            {
                throw new EmptyListException();
            }

            var character = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterId == characterId);
            if (character == null)
            {
                throw new CharacterNotFoundException(characterId);
            }

            var allItems = await _context.Items.ToListAsync();
            var requestedItemsIds = dto.ItemIds.Distinct().ToList();
            var allItemIds = allItems.Select(i => i.ItemId).ToList();
            var missingItemsIds = new List<int>();

            foreach (var itemId in requestedItemsIds)
            {
                if (!allItemIds.Contains(itemId))
                {
                    missingItemsIds.Add(itemId);
                }
            }

            if (missingItemsIds.Any())
            {
                throw new ItemNotFoundException(missingItemsIds);
            }

            var itemsToAdd = allItems.Where(i => requestedItemsIds.Contains(i.ItemId));
            int totalWeightToAdd = itemsToAdd.Sum(i => i.Weight);
            int availableWeight = character.MaxWeight = character.CurrentWeight;
            if (totalWeightToAdd > availableWeight)
            {
                throw new ToMuchWeightException();
            }

            foreach (var item in itemsToAdd)
            {
                var existing = await _context.Backpacks.FirstOrDefaultAsync(b =>
                    b.CharacterId == character.CharacterId && b.ItemId == item.ItemId);
                if (existing != null)
                {
                    existing.Amount += 1;
                }
                else
                {
                    _context.Backpacks.Add(new Backpack()
                    {
                        CharacterId = character.CharacterId,
                        ItemId = item.ItemId,
                        Amount = 1
                    });
                }
            }

            character.CurrentWeight += totalWeightToAdd;
            await _context.SaveChangesAsync();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}