namespace Cwiczenia12.Exceptions;

public class ClientHasTripsException : Exception
{
    public ClientHasTripsException(int clientId)
        : base($"Client with ID {clientId} cannot be deleted because it has assigned trips") { }
}