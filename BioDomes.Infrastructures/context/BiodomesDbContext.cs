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
        // Ajout des régimes
        modelBuilder.Entity<SpeciesRegime>().HasData(
            new SpeciesRegime { IdRegime = "1", Name = "Photosynthèse" },
            new SpeciesRegime { IdRegime = "2", Name = "Carnivore" },
            new SpeciesRegime { IdRegime = "3", Name = "Omnivore" },
            new SpeciesRegime { IdRegime = "4", Name = "Coprophage" },
            new SpeciesRegime { IdRegime = "5", Name = "Herbivore" },
            new SpeciesRegime { IdRegime = "6", Name = "Détritivores" },
            new SpeciesRegime { IdRegime = "7", Name = "Hétérotrophie" },
            new SpeciesRegime { IdRegime = "8", Name = "Coprophages" }
        );

        // Ajout des types d'especes
        modelBuilder.Entity<SpeciesType>().HasData(
            new SpeciesType { IdType = "1", Name = "Mammiferes" },
            new SpeciesType { IdType = "2", Name = "Oiseaux" },
            new SpeciesType { IdType = "3", Name = "Poissons" },
            new SpeciesType { IdType = "4", Name = "Reptiles" },
            new SpeciesType { IdType = "5", Name = "Amphibiens" },
            new SpeciesType { IdType = "6", Name = "Crustaces" },
            new SpeciesType { IdType = "7", Name = "Arachnides" },
            new SpeciesType { IdType = "8", Name = "Insectes" },
            new SpeciesType { IdType = "9", Name = "Myriapodes" },
            new SpeciesType { IdType = "10", Name = "Mollusques" },
            new SpeciesType { IdType = "11", Name = "Echinodermes" },
            new SpeciesType { IdType = "12", Name = "Anelides" },
            new SpeciesType { IdType = "13", Name = "Cnidaires" },
            new SpeciesType { IdType = "14", Name = "Plathelminthes" },
            new SpeciesType { IdType = "15", Name = "Poriferes" },
            new SpeciesType { IdType = "16", Name = "Plantes" },
            new SpeciesType { IdType = "17", Name = "Mousses" },
            new SpeciesType { IdType = "18", Name = "Algues" },
            new SpeciesType { IdType = "19", Name = "Lichen" },
            new SpeciesType { IdType = "20", Name = "Champignons" }
        );

        modelBuilder.Entity<Species>().HasData(
            new Species
            {
                IdSpecies = "1",
                Name = "Lion",
                RegimId = "2", // Carnivore
                TypeId = "1", // Mammifere
                Visible = true,
                Avatar = "lion.webp",
                Size = 180,
                Weight = 150000,
                UserId = null
            },
            new Species
            {
                IdSpecies = "2",
                Name = "Boa",
                RegimId = "2", // Carnivore
                TypeId = "4", // Reptile
                Visible = true,
                Avatar = "boa.webp",
                Size = 225,
                Weight = 13000,
                UserId = null
            },
            new Species
            {
                IdSpecies = "3",
                Name = "Hetre",
                RegimId = "1", // Photosynthèse
                TypeId = "16", // Plante
                Visible = true,
                Avatar = "hetre.webp",
                Size = 600,
                Weight = 830000,
                UserId = null
            },
            new Species
            {
                IdSpecies = "4",
                Name = "Chene",
                RegimId = "1", // Photosynthèse
                TypeId = "16", // Plante
                Visible = true,
                Avatar = "chene.webp",
                Size = 2500,
                Weight = 2100000,
                UserId = null
            },
            new Species
            {
                IdSpecies = "5",
                Name = "Mouche",
                RegimId = "4", // Coprophage
                TypeId = "8", // Insecte
                Visible = true,
                Avatar = "mouche.webp",
                Size = 1,
                Weight = 1,
                UserId = null
            },
            new Species
            {
                IdSpecies = "6",
                Name = "Araignée",
                RegimId = "2", // Carnivore
                TypeId = "7", // Arachnide
                Visible = true,
                Avatar = "araignee.webp",
                Size = 4,
                Weight = 95,
                UserId = null
            },
            new Species
            {
                IdSpecies = "7",
                Name = "Hirondelle",
                RegimId = "2", // Carnivore
                TypeId = "2", // Oiseau
                Visible = true,
                Avatar = "hirondelle.webp",
                Size = 18,
                Weight = 45,
                UserId = null
            },
            new Species
            {
                IdSpecies = "8",
                Name = "Lievre",
                RegimId = "5", // Herbivore
                TypeId = "1", // Mammifere
                Visible = true,
                Avatar = "lievre.webp",
                Size = 50,
                Weight = 4000,
                UserId = null
            },
            new Species
            {
                IdSpecies = "9",
                Name = "Pâturin des prés",
                RegimId = "1", // Photosynthèse
                TypeId = "16", // Plante
                Visible = true,
                Avatar = "paturin.webp",
                Size = 23,
                Weight = 1,
                UserId = null
            },
            new Species
            {
                IdSpecies = "10",
                Name = "Souris",
                RegimId = "3", // Omnivore
                TypeId = "1", // Mammifere
                Visible = true,
                Avatar = "souris.webp",
                Size = 9,
                Weight = 25,
                UserId = null
            }
        );

        modelBuilder.Entity<Biodomes>().HasData(
            new Biodomes
            {
                idBiodomes = "1",
                Name = "Forêt Tempérée",
                Temperature = 18,
                Humidity = 70,
                Avatar = "/wwwroot/images/biodomes/1.wepb",
                userId = null
            },
            new Biodomes
            {
                idBiodomes = "2",
                Name = "Forêt Tropicale",
                Temperature = 28,
                Humidity = 90,
                Avatar = "/wwwroot/images/biodomes/1.wepb",
                userId = null
            },
            new Biodomes
            {
                idBiodomes = "3",
                Name = "Savane",
                Temperature = 32,
                Humidity = 40,
                Avatar = "/wwwroot/images/biodomes/1.wepb",
                userId = null
            },
            new Biodomes
            {
                idBiodomes = "4",
                Name = "Prairie Tempérée",
                Temperature = 20,
                Humidity = 55,
                Avatar = "/wwwroot/images/biodomes/1.wepb",
                userId = null
            },
            new Biodomes
            {
                idBiodomes = "5",
                Name = "Laboratoire Contrôlé",
                Temperature = 22,
                Humidity = 60,
                Avatar = "/wwwroot/images/biodomes/1.wepb",
                userId = null
            }
        );
    }
}