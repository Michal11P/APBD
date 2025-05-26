namespace Cwiczenia12.Exceptions;

public class TripNotfoundException:Exception
{
    public TripNotfoundException(int tripId)
        :base($"Trip with ID: {tripId} does not exist or has already started"){}
}