namespace Cwiczenia12.Exceptions;

public class ClientAlreadyAssignedException : Exception
{
    public ClientAlreadyAssignedException(string pesel, int tripId)
        : base($"Client with PESEL: {pesel} has already been assigned to trip with ID: {tripId}"){}

}