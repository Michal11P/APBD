using Cwiczenia11.Data;
using Cwiczenia11.DTOs;
using Cwiczenia11.Exceptions;
using Cwiczenia11.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia11.Services;

public class PatientService:IPatientService 
{
    private readonly PrescriptionDbContext _context;

    public PatientService(PrescriptionDbContext context)
    {
        _context = context;
    }

    public async Task<PatientDetailsDTO> GetPatientDetails(int patientId)
    {
        var patient = await _context.Patients
            .Include(p=>p.Prescriptions)
            .ThenInclude(d=>d.Doctor)
            .Include(p=>p.Prescriptions)
            .ThenInclude(pm=>pm.PrescriptionsMedicaments)
            .ThenInclude(m=>m.Medicament)
            .FirstOrDefaultAsync(p=>p.IdPatient == patientId);
        if (patient == null)
        {
            throw new PatientNotFoundException(patientId);
        }

        var patientDetails = new PatientDetailsDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDetailsDTO
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.DueDate,
                    DueDate = p.DueDate,
                    Medicaments = p.PrescriptionsMedicaments.Select(pm=>new MedicamentFullDTO
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose ?? 0
                    }).ToList(),
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = p.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName
                    }
                }).ToList()
        };
        return patientDetails;
    }
}