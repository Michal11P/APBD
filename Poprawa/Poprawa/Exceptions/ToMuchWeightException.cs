namespace Poprawa.Exceptions;

public class ToMuchWeightException:Exception
{
    public ToMuchWeightException() : base("Character does not have enough available weight") { }
}