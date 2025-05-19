using Cwiczenia11.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia11.Data;

public class PrescriptionDbContext : DbContext
{
    protected PrescriptionDbContext(){}
    public PrescriptionDbContext(DbContextOptions options) : base(options) {}
    
    
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionsMedicaments { get; set; }
}