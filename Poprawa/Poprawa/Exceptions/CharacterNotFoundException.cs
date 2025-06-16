namespace Poprawa.Exceptions;

public class CharacterNotFoundException:Exception
{
    public CharacterNotFoundException(int characterId):
        base($"Character with id {characterId} not found." ){}
}