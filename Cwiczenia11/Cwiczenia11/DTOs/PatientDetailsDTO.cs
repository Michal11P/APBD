namespace Cwiczenia11.DTOs;

public class PatientDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionDetailsDTO> Prescriptions { get; set; }
}

public class PrescriptionDetailsDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentFullDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class MedicamentFullDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int? Dose { get; set; }
    public string Description { get; set; }
}