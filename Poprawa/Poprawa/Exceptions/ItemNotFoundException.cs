namespace Poprawa.Exceptions;

public class ItemNotFoundException:Exception
{
    public ItemNotFoundException(List<int>itemsIds):
        base($"Items with ids: {itemsIds} was not found"){}
}