using Cwiczenia11.Data;
using Cwiczenia11.DTOs;
using Cwiczenia11.Exceptions;
using Cwiczenia11.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia11.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly PrescriptionDbContext _context;

    public PrescriptionService(PrescriptionDbContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(NewPrescription newPrescription)
    {
        if (newPrescription.Medicaments.Count > 10)
        {
            throw new TooManyMedicamentsException();
        }

        if (newPrescription.DueDate < newPrescription.Date)
        {
            throw new InvalitDateRangeException();
        }
        
        var doctorExists =await _context.Doctors.AnyAsync(d => d.IdDoctor==newPrescription.IdDoctor);
        if (!doctorExists)
        {
            throw new DoctorNotFoundException(newPrescription.IdDoctor);
        }

        var patient = await _context.Patients.FindAsync(newPrescription.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = newPrescription.Patient.FirstName,
                LastName = newPrescription.Patient.LastName,
                BirthDate = newPrescription.Patient.BirthDate,
            };
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        foreach (var medicament in newPrescription.Medicaments)
        {
            var medicamentExists = await _context.Medicaments.AnyAsync(m=>m.IdMedicament == medicament.IdMedicament);
            if (!medicamentExists)
            {
                throw new MedicamentNotFoundException(medicament.IdMedicament);
            }
        }

        var prescription = new Prescription
        {
            Date = newPrescription.Date,
            DueDate = newPrescription.DueDate,
            IdDoctor = newPrescription.IdDoctor,
            IdPatient = patient.IdPatient,
        };
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();

        foreach (var medicament in newPrescription.Medicaments)
        {
            var prescriptionMedicament = new Prescription_Medicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = medicament.IdMedicament,
                Dose = medicament.Dose,
                Details = medicament.Description
            };
            await _context.PrescriptionsMedicaments.AddAsync(prescriptionMedicament);
        }
        await _context.SaveChangesAsync();
    }
}