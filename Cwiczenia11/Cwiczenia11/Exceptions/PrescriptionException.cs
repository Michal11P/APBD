namespace Cwiczenia11.Exceptions;

public class PrescriptionException : Exception
{
    public PrescriptionException(string message) : base(message){}
}

public class DoctorNotFoundException : PrescriptionException
{
    public DoctorNotFoundException(int doctorId) : base($"Doctor with ID {doctorId} not found"){}
}

public class MedicamentNotFoundException : PrescriptionException
{
    public MedicamentNotFoundException(int medicamentId) : base($"Medicament with ID {medicamentId} not found"){}
}

public class TooManyMedicamentsException : PrescriptionException
{
    public TooManyMedicamentsException(): base("Prescription cannot have more than 10 medicaments"){}
}

public class InvalitDateRangeException : PrescriptionException
{
    public InvalitDateRangeException():base("DueDate must be greater or equal to Date"){}
}