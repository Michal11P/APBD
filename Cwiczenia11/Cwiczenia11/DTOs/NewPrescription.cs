using Cwiczenia11.Models;

namespace Cwiczenia11.DTOs;

public class NewPrescription
{
   public PatientDTO Patient { get; set; }
   public List<MedicamentDTO> Medicaments { get; set; }
   public DateTime Date { get; set; }
   public DateTime DueDate { get; set; }
   public int IdDoctor { get; set; }
}

public class PatientDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}

public class MedicamentDTO
{
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    public string Description { get; set; }

}
