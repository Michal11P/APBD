using Microsoft.EntityFrameworkCore;
using Poprawa.Models;

namespace Poprawa.Data;

public class DatabaseContext:DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Title> Titles { get; set; }
    public DbSet<Character_Title> Character_Titles { get; set; }

    protected DatabaseContext()
    {
        
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().HasData(new List<Item>()
        {
            new Item(){ItemId = 1, Name ="Sword", Weight = 12 },
            new Item(){ItemId = 2, Name ="Shield", Weight = 20 },
            new Item(){ItemId = 3, Name ="Axe", Weight = 15 },
            new Item(){ItemId = 4, Name ="Bow", Weight = 8 }
        });

        modelBuilder.Entity<Character>().HasData(new List<Character>()
        {
            new Character() {CharacterId = 1, FirstName = "Aragorn", LastName = "Son of Arathorn", CurrentWeight = 12, MaxWeight = 50 },
            new Character() { CharacterId = 2, FirstName = "Legolas", LastName = "Greenleaf", CurrentWeight = 5, MaxWeight = 40 },
            new Character() { CharacterId = 3, FirstName = "Gimli", LastName = "Son of Gloin", CurrentWeight = 25, MaxWeight = 60 }
        });

        modelBuilder.Entity<Backpack>().HasData(new List<Backpack>()
        {
            new Backpack(){CharacterId = 1, ItemId = 1, Amount = 1},
            new Backpack(){CharacterId = 2, ItemId = 4, Amount = 1},
            new Backpack(){CharacterId = 3, ItemId = 3, Amount = 1},
            new Backpack(){CharacterId = 3, ItemId = 2, Amount = 1},
        });

        modelBuilder.Entity<Title>().HasData(new List<Title>()
        {
            new Title() { TitleId = 1, Name = "Ranger" },
            new Title() { TitleId = 2, Name = "Elven Prince" },
            new Title() { TitleId = 3, Name = "Dwarf Warrior" }
        });

        modelBuilder.Entity<Character_Title>().HasData(new List<Character_Title>()
        {
            new Character_Title() { CharacterId = 1, TitleId = 1, AcquiredAt = DateTime.Parse("2020-06-01 10:00:00") },
            new Character_Title() { CharacterId = 2, TitleId = 2, AcquiredAt = DateTime.Parse("2021-01-15 14:30:00") },
            new Character_Title() { CharacterId = 3, TitleId = 3, AcquiredAt = DateTime.Parse("2019-09-20 08:45:00") }
        });

    }
}