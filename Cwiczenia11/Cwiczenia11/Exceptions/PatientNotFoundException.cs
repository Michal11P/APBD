namespace Cwiczenia11.Exceptions;

public class PatientNotFoundException : PrescriptionException
{
    public PatientNotFoundException(int patientId) : base($"Patient with ID {patientId} does not exist"){}
}