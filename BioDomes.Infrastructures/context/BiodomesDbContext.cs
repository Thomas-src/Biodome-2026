using BioDomes.Infrastructures.configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;

namespace BioDomes.Infrastructures.context;

public class BioDomesDbContext : IdentityDbContext<UserEntity>
{
    public BioDomesDbContext(DbContextOptions<BioDomesDbContext> options)
        : base(options)
    {
    }

    private static SpeciesEntity souris = new SpeciesEntity
    {
        IdSpecies = 10,
        Name = "Souris",
        RegimId = 3, // Omnivore
        TypeId = 1, // Mammifere
        Visible = true,
        Avatar = "souris.webp",
        Size = 9,
        Weight = 25,
        UserId = null
    };

    public DbSet<BiodomeEntity> Biodomes { get; set; }
    public DbSet<SpeciesEntity> Species { get; set; }
    public DbSet<EquipmentEntity> Equipment { get; set; }
    public DbSet<ResearchOrganismEntity> ResearchOrganisms { get; set; }
    public DbSet<SpeciesTypeEntity> SpeciesTypes { get; set; }
    public DbSet<SpeciesRegimeEntity> SpeciesRegim { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new ResearchOrganismConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityRoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        SeedUsersData(modelBuilder);
        // Les clés primaire
        modelBuilder.Entity<BiodomeEntity>()
            .HasKey(b => b.idBiodomes);

        modelBuilder.Entity<SpeciesEntity>()
            .HasKey(s => s.IdSpecies);

        modelBuilder.Entity<EquipmentEntity>()
            .HasKey(e => e.IdEquipment);

        modelBuilder.Entity<ResearchOrganismEntity>()
            .HasKey(r => r.ResearchOrganismId);

        modelBuilder.Entity<SpeciesRegimeEntity>()
            .HasKey(r => r.IdRegime);

        modelBuilder.Entity<SpeciesTypeEntity>()
            .HasKey(r => r.IdType);

        // User -> Biodomes
        modelBuilder.Entity<BiodomeEntity>()
            .HasOne(b => b.user)
            .WithMany(u => u.Biodomes)
            .HasForeignKey(b => b.userId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Equipement
        modelBuilder.Entity<EquipmentEntity>()
            .HasOne(e => e.User)
            .WithMany(u => u.Equipments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // User -> Species
        modelBuilder.Entity<SpeciesEntity>()
            .HasOne(s => s.User)
            .WithMany(u => u.Species)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // ResearchOrganism -> User
        /*
         * 
         */
     modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.ResearchOrganism)
            .WithMany(r => r.Users)
            //.HasForeignKey(u => u.ResearchOrganismId)
            .OnDelete(DeleteBehavior.Restrict);
       

        // Biodomes <-> Species
        modelBuilder.Entity<BiodomeEntity>()
            .HasMany(b => b.Species)
            .WithMany(s => s.Biodomes)
            .UsingEntity<Dictionary<string, object>>(
                "BiodomeSpecies",
                j => j
                    .HasOne<SpeciesEntity>()
                    .WithMany()
                    .HasForeignKey("SpeciesId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<BiodomeEntity>()
                    .WithMany()
                    .HasForeignKey("BiodomeId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        // Biodomes <-> Equipment
        modelBuilder.Entity<BiodomeEntity>()
            .HasMany(b => b.Equipment)
            .WithMany(e => e.Biodomes)
            .UsingEntity<Dictionary<string, object>>(
                "BiodomeEquipment",
                j => j
                    .HasOne<EquipmentEntity>()
                    .WithMany()
                    .HasForeignKey("EquipmentId"),
                j => j
                    .HasOne<BiodomeEntity>()
                    .WithMany()
                    .HasForeignKey("BiodomeId")
            );

        modelBuilder.Entity<SpeciesEntity>()
            .HasOne(r => r.Regim)
            .WithMany(s => s.Species)
            .HasForeignKey(r => r.RegimId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SpeciesEntity>()
            .HasOne(r => r.TypeEntity)
            .WithMany(s => s.Species)
            .HasForeignKey(r => r.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(modelBuilder);
    }

    public static void SeedData(ModelBuilder modelBuilder)
    {
        // Ajout des régimes
        modelBuilder.Entity<SpeciesRegimeEntity>().HasData(
            new SpeciesRegimeEntity { IdRegime = 1, Name = "Photosynthèse" },
            new SpeciesRegimeEntity { IdRegime = 2, Name = "Carnivore" },
            new SpeciesRegimeEntity { IdRegime = 3, Name = "Omnivore" },
            new SpeciesRegimeEntity { IdRegime = 4, Name = "Coprophage" },
            new SpeciesRegimeEntity { IdRegime = 5, Name = "Herbivore" },
            new SpeciesRegimeEntity { IdRegime = 6, Name = "Détritivores" },
            new SpeciesRegimeEntity { IdRegime = 7, Name = "Hétérotrophie" },
            new SpeciesRegimeEntity { IdRegime = 8, Name = "Coprophages" }
        );

        // Ajout des types d'especes
        modelBuilder.Entity<SpeciesTypeEntity>().HasData(
            new SpeciesTypeEntity { IdType = 1, Name = "Mammiferes" },
            new SpeciesTypeEntity { IdType = 2, Name = "Oiseaux" },
            new SpeciesTypeEntity { IdType = 3, Name = "Poissons" },
            new SpeciesTypeEntity { IdType = 4, Name = "Reptiles" },
            new SpeciesTypeEntity { IdType = 5, Name = "Amphibiens" },
            new SpeciesTypeEntity { IdType = 6, Name = "Crustaces" },
            new SpeciesTypeEntity { IdType = 7, Name = "Arachnides" },
            new SpeciesTypeEntity { IdType = 8, Name = "Insectes" },
            new SpeciesTypeEntity { IdType = 9, Name = "Myriapodes" },
            new SpeciesTypeEntity { IdType = 10, Name = "Mollusques" },
            new SpeciesTypeEntity { IdType = 11, Name = "Echinodermes" },
            new SpeciesTypeEntity { IdType = 12, Name = "Anelides" },
            new SpeciesTypeEntity { IdType = 13, Name = "Cnidaires" },
            new SpeciesTypeEntity { IdType = 14, Name = "Plathelminthes" },
            new SpeciesTypeEntity { IdType = 15, Name = "Poriferes" },
            new SpeciesTypeEntity { IdType = 16, Name = "Plantes" },
            new SpeciesTypeEntity { IdType = 17, Name = "Mousses" },
            new SpeciesTypeEntity { IdType = 18, Name = "Algues" },
            new SpeciesTypeEntity { IdType = 19, Name = "Lichen" },
            new SpeciesTypeEntity { IdType = 20, Name = "Champignons" }
        );

        modelBuilder.Entity<SpeciesEntity>().HasData(
            new SpeciesEntity
            {
                IdSpecies = 1,
                Name = "Lion",
                RegimId = 2, // Carnivore
                TypeId = 1, // Mammifere
                Visible = true,
                Avatar = "/images/species/lion.jpg",
                Size = 180,
                Weight = 150000,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 2,
                Name = "Boa",
                RegimId = 2, // Carnivore
                TypeId = 4, // Reptile
                Visible = true,
                Avatar = "boa.webp",
                Size = 225,
                Weight = 13000,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 3,
                Name = "Hetre",
                RegimId = 1, // Photosynthèse
                TypeId = 16, // Plante
                Visible = true,
                Avatar = "hetre.webp",
                Size = 600,
                Weight = 830000,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 4,
                Name = "Chene",
                RegimId = 1, // Photosynthèse
                TypeId = 16, // Plante
                Visible = true,
                Avatar = "chene.webp",
                Size = 2500,
                Weight = 2100000,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 5,
                Name = "Mouche",
                RegimId = 4, // Coprophage
                TypeId = 8, // Insecte
                Visible = true,
                Avatar = "mouche.webp",
                Size = 1,
                Weight = 1,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 6,
                Name = "Araignée",
                RegimId = 2, // Carnivore
                TypeId = 7, // Arachnide
                Visible = true,
                Avatar = "araignee.webp",
                Size = 4,
                Weight = 95,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 7,
                Name = "Hirondelle",
                RegimId = 2, // Carnivore
                TypeId = 2, // Oiseau
                Visible = true,
                Avatar = "hirondelle.webp",
                Size = 18,
                Weight = 45,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 8,
                Name = "Lievre",
                RegimId = 5, // Herbivore
                TypeId = 1, // Mammifere
                Visible = true,
                Avatar = "lievre.webp",
                Size = 50,
                Weight = 4000,
                UserId = null
            },
            new SpeciesEntity
            {
                IdSpecies = 9,
                Name = "Pâturin des prés",
                RegimId = 1, // Photosynthèse
                TypeId = 16, // Plante
                Visible = true,
                Avatar = "paturin.webp",
                Size = 23,
                Weight = 1,
                UserId = null
            },
            souris
            
        );

        var list =  new List<SpeciesEntity> { souris };
        modelBuilder.Entity<BiodomeEntity>().HasData(
            new BiodomeEntity
            {
                idBiodomes = 1,
                Name = "Forêt Tempérée",
                Temperature = 18,
                Humidity = 70,
                Avatar = "foret-temperee.png",
                userId = null
            },
            new BiodomeEntity
            {
                idBiodomes = 2,
                Name = "Forêt Tropicale",
                Temperature = 28,
                Humidity = 90,
                Avatar = "foret-tropicale.png",
                userId = null
            },
            new BiodomeEntity
            {
                idBiodomes = 3,
                Name = "Savane",
                Temperature = 32,
                Humidity = 40,
                Avatar = "savane.png",
                userId = null
            },
            new BiodomeEntity
            {
                idBiodomes = 4,
                Name = "Prairie Tempérée",
                Temperature = 20,
                Humidity = 55,
                Avatar = "prarie.png",
                userId = null
            },
            new BiodomeEntity
            {
                idBiodomes = 5,
                Name = "Laboratoire Contrôlé",
                Temperature = 22,
                Humidity = 60,
                Avatar = "labo.png",
                userId = null
            }
        );
        modelBuilder.Entity("BiodomeSpecies").HasData(
            new { BiodomeId = 2, SpeciesId = 10 }, // souris
        new { BiodomeId = 2, SpeciesId = 9 }
        );
    }

    private static void SeedUsersData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = UserConfiguration.AdminId,
                RoleId = IdentityRoleConfiguration.AdminId
            },
            new IdentityUserRole<string>
            {
                UserId = UserConfiguration.ResearchId,
                RoleId = IdentityRoleConfiguration.ResearchID
            }
        );
    }
}