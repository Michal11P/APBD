namespace Cwiczenia3;

public class CreateContainer
{
    private static int uniqueContainerNumber = 1;

    public static string GenerateSerialNumber(string containerType)
    {
        return $"KON-{containerType}-{uniqueContainerNumber}";
    }
}