using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Data;

public class DatabaseContext:DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ProgramEntity> Programs { get; set; }
    public DbSet<Washing_Machine> WashingMachines { get; set; }
    public DbSet<Purchase_History> PurchasesHistory { get; set; }
    public DbSet<Available_Program> AvailablePrograms { get; set; } 
    
    protected DatabaseContext()
    {
    }
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(new List<Customer>()
        {
            new Customer() { CustomerId = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "123456789" },
            new Customer() { CustomerId = 2, FirstName = "Jane", LastName = "Doe", PhoneNumber = "123098754" },
            new Customer() { CustomerId = 3, FirstName = "Julie", LastName = "Doe", PhoneNumber = "123456654321" }
        });

        modelBuilder.Entity<Washing_Machine>().HasData(new List<Washing_Machine>()
        {
            new Washing_Machine() { WashingMachineId = 1, MaxWeight = 50.0, SerialNumber = "123" },
            new Washing_Machine() { WashingMachineId = 2, MaxWeight = 20.5, SerialNumber = "321" },
            new Washing_Machine() { WashingMachineId = 3, MaxWeight = 12.6, SerialNumber = "098" }
        });

        modelBuilder.Entity<ProgramEntity>().HasData(new List<ProgramEntity>()
        {
            new ProgramEntity() { ProgramId = 1, Name = "Fast", DurationMinutes = 15, TemperatureCelsius = 50 },
            new ProgramEntity() { ProgramId = 2, Name = "Medium", DurationMinutes = 25, TemperatureCelsius = 60 },
            new ProgramEntity() { ProgramId = 3, Name = "Slow", DurationMinutes = 60, TemperatureCelsius = 40 }
        });

        modelBuilder.Entity<Available_Program>().HasData(new List<Available_Program>()
        {
            new Available_Program() { AvailableProgramId = 1, WashingMachineId = 1, ProgramId = 2, Price = 100 },
            new Available_Program() { AvailableProgramId = 2, WashingMachineId = 3, ProgramId = 1, Price = 50 },
            new Available_Program() { AvailableProgramId = 3, WashingMachineId = 2, ProgramId = 3, Price = 75 }
        });

        modelBuilder.Entity<Purchase_History>().HasData(new List<Purchase_History>()
        {
            new Purchase_History()
                { AvailableProgramId = 1, CustomerId = 1, PurchaseDate = DateTime.Parse("2025-05-01"), Rating = 5 },
            new Purchase_History()
                { AvailableProgramId = 2, CustomerId = 2, PurchaseDate = DateTime.Parse("2025-06-01"), Rating = 3 },
            new Purchase_History()
                { AvailableProgramId = 3, CustomerId = 3, PurchaseDate = DateTime.Parse("2025-05-15"), Rating = 4 }
        });
    }
}