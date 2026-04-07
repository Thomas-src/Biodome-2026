using BioDomes.Infrastructures.entities;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.context;

public class BiodomeContext : DbContext
{
    public DbSet<Biodomes> Biodomes { get; set; }
    public DbSet<Species> Species { get; set; }

    public BiodomeContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Biodomes>().HasKey(b => b.idBiodomes);
        modelBuilder.Entity<Species>().HasKey(s => s.IdSpecies);

        modelBuilder.Entity<Biodomes>()
            .HasMany(b => b.Species)
            .WithOne()
            .HasForeignKey("BiodomeId");

        SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Biodomes>().HasData(
            new Biodomes
            {
                idBiodomes = "1",
                Name = "Biodome Alpha"
            }
        );

        modelBuilder.Entity<Species>().HasData(
            new Species
            {
                IdSpecies = "E220291",
                Name = "Chien",
                Regim = "Carnivore",
                Visible = true,
                Avatar = "chien.webp",
                Size = 2,
                Type = "Canin",
                Weight = 20
            }
        );
    }
}