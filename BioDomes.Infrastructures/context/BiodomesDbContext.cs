using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BioDomes.Infrastructures.entities;

namespace BioDomes.Infrastructures.context;

public class BioDomesDbContext : IdentityDbContext<User>
{
    public BioDomesDbContext(DbContextOptions<BioDomesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Biodomes> Biodomes { get; set; }
    public DbSet<Species> Species { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<ResearchOrganism> ResearchOrganisms { get; set; }
    public DbSet<SpeciesType> SpeciesTypes { get; set; }
    public DbSet<SpeciesRegime> SpeciesRegim { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Les clés primaire
        modelBuilder.Entity<Biodomes>()
            .HasKey(b => b.idBiodomes);

        modelBuilder.Entity<Species>()
            .HasKey(s => s.IdSpecies);

        modelBuilder.Entity<Equipment>()
            .HasKey(e => e.IdEquipment);

        modelBuilder.Entity<ResearchOrganism>()
            .HasKey(r => r.IdOrgansim);

        modelBuilder.Entity<SpeciesRegime>()
            .HasKey(r => r.IdRegime);

        modelBuilder.Entity<SpeciesType>()
            .HasKey(r => r.IdType);

        // User -> Biodomes
        modelBuilder.Entity<Biodomes>()
            .HasOne(b => b.user)
            .WithMany(u => u.Biodomes)
            .HasForeignKey(b => b.userId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Equipement
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.User)
            .WithMany(u => u.Equipments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // User -> Species
        modelBuilder.Entity<Species>()
            .HasOne(s => s.User)
            .WithMany(u => u.Species)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // ResearchOrganism -> User
        modelBuilder.Entity<User>()
            .HasOne(u => u.ResearchOrganism)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.ResearchOrganismeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Biodomes <-> Species
        modelBuilder.Entity<Biodomes>()
            .HasMany(b => b.Species)
            .WithMany(s => s.Biodomes)
            .UsingEntity<Dictionary<string, object>>(
                "BiodomeSpecies",
                j => j
                    .HasOne<Species>()
                    .WithMany()
                    .HasForeignKey("SpeciesId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Biodomes>()
                    .WithMany()
                    .HasForeignKey("BiodomeId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        // Biodomes <-> Equipment
        modelBuilder.Entity<Biodomes>()
            .HasMany(b => b.Equipment)
            .WithMany(e => e.Biodomes)
            .UsingEntity<Dictionary<string, object>>(
                "BiodomeEquipment",
                j => j
                    .HasOne<Equipment>()
                    .WithMany()
                    .HasForeignKey("EquipmentId"),
                j => j
                    .HasOne<Biodomes>()
                    .WithMany()
                    .HasForeignKey("BiodomeId")
            );

        modelBuilder.Entity<Species>()
            .HasOne(r => r.Regim)
            .WithMany(s => s.Species)
            .HasForeignKey(r => r.RegimId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Species>()
            .HasOne(r => r.Type)
            .WithMany(s => s.Species)
            .HasForeignKey(r => r.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(modelBuilder);
    }

    public static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Biodomes>().HasData(
            new Biodomes
            {
                idBiodomes = "1",
                Name = "Biodome Alpha",
                Temperature = 22.5,
                Humidity = 60,
                Avatar = "alpha.png",
                userId = null
            },
            new Biodomes
            {
                idBiodomes = "2",
                Name = "Biodome Lune",
                Temperature = -10,
                Humidity = 5,
                Avatar = "moon.png",
                userId = null
            },
            new Biodomes
            {
                idBiodomes = "3",
                Name = "Biodome Mars",
                Temperature = -20,
                Humidity = 2,
                Avatar = "mars.png",
                userId = null
            }
        );

        modelBuilder.Entity<SpeciesRegime>().HasData(
            new SpeciesRegime { IdRegime = "1", Name = "Carnivore" },
            new SpeciesRegime { IdRegime = "2", Name = "Herbivore" },
            new SpeciesRegime { IdRegime = "3", Name = "Omnivore" }
        );

        modelBuilder.Entity<SpeciesType>().HasData(
            new SpeciesType { IdType = "1", Name = "Canin" },
            new SpeciesType { IdType = "2", Name = "Félin" },
            new SpeciesType { IdType = "3", Name = "Reptile" }
        );

        modelBuilder.Entity<Species>().HasData(
            new Species
            {
                IdSpecies = "E220291",
                Name = "Chien",
                RegimId = "1",   // Carnivore
                TypeId = "1",    // Canin
                Visible = true,
                Avatar = "chien.webp",
                Size = 2,
                Weight = 20,
                UserId = null
            }
        );
    }
}