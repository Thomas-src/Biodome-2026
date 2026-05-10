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

    private static readonly SpeciesEntity Souris = new SpeciesEntity
    {
        IdSpecies = 10,
        Name = "Souris",
        RegimId = 3, // Omnivore
        TypeId = 1, // Mammifere
        Visible = true,
        Avatar = "souris.jpg",
        Size = 9,
        Weight = 25,
        UserId = "2"
    };

    public DbSet<BiodomeEntity> Biodomes { get; set; }
    public DbSet<SpeciesEntity> Species { get; set; }
    public DbSet<EquipmentEntity> Equipment { get; set; }
    public DbSet<ResearchOrganismEntity> ResearchOrganisms { get; set; }
    public DbSet<SpeciesTypeEntity> SpeciesTypes { get; set; }
    public DbSet<SpeciesRegimeEntity> SpeciesRegimes { get; set; }

    public DbSet<ConsumptionEntity> Consumptions { get; set; }

    public DbSet<ProductionEntity> Productions { get; set; }

    public DbSet<BiodomeSpeciesEntity> BiodomeSpecies { get; set; }

    public DbSet<BiodomeHistoryEntity> BiodomeHistory { get; set; }

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

        modelBuilder.Entity<ProductionEntity>()
            .HasKey(p => p.ProductionId);

        modelBuilder.Entity<ConsumptionEntity>()
            .HasKey(c => c.ConsumptionId);

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

        modelBuilder.Entity<EquipmentEntity>()
            .HasOne(e => e.Production)
            .WithMany(p => p.Equipments)
            .HasForeignKey(e => e.ProductionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EquipmentEntity>()
            .HasOne(e => e.Consumption)
            .WithMany(c => c.Equipments)
            .HasForeignKey(e => e.ConsumptionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

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
        modelBuilder.Entity<BiodomeSpeciesEntity>()
            .HasKey(bs => new { bs.BiodomeId, bs.SpeciesId });

        modelBuilder.Entity<BiodomeSpeciesEntity>()
            .HasOne(bs => bs.Biodome)
            .WithMany(b => b.BiodomeSpecies)
            .HasForeignKey(bs => bs.BiodomeId);

        modelBuilder.Entity<BiodomeSpeciesEntity>()
            .HasOne(bs => bs.Species)
            .WithMany(s => s.BiodomeSpecies)
            .HasForeignKey(bs => bs.SpeciesId);

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
                Avatar = "lion.jpg",
                Size = 180,
                Weight = 150000,
                UserId = "2"
            },
            new SpeciesEntity
            {
                IdSpecies = 2,
                Name = "Boa",
                RegimId = 2, // Carnivore
                TypeId = 4, // Reptile
                Visible = true,
                Avatar = "boa.jpg",
                Size = 225,
                Weight = 13000,
                UserId = "2"
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
                UserId = "2"
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
                UserId = "2"
            },
            new SpeciesEntity
            {
                IdSpecies = 5,
                Name = "Mouche",
                RegimId = 4, // Coprophage
                TypeId = 8, // Insecte
                Visible = true,
                Avatar = "mouche.jpg",
                Size = 1,
                Weight = 1,
                UserId = "2"
            },
            new SpeciesEntity
            {
                IdSpecies = 6,
                Name = "Araignée",
                RegimId = 2, // Carnivore
                TypeId = 7, // Arachnide
                Visible = true,
                Avatar = "araigne.jpg",
                Size = 4,
                Weight = 95,
                UserId = "2"
            },
            new SpeciesEntity
            {
                IdSpecies = 7,
                Name = "Hirondelle",
                RegimId = 2, // Carnivore
                TypeId = 2, // Oiseau
                Visible = true,
                Avatar = "hirondelle.jpg",
                Size = 18,
                Weight = 45,
                UserId = "2"
            },
            new SpeciesEntity
            {
                IdSpecies = 8,
                Name = "Lievre",
                RegimId = 5, // Herbivore
                TypeId = 1, // Mammifere
                Visible = true,
                Avatar = "lievrejpeg.jpeg",
                Size = 50,
                Weight = 4000,
                UserId = "2"
            },
            new SpeciesEntity
            {
                IdSpecies = 9,
                Name = "Pâturin des prés",
                RegimId = 1, // Photosynthèse
                TypeId = 16, // Plante
                Visible = true,
                Avatar = "paturin.jpg",
                Size = 23,
                Weight = 1,
                UserId = "2"
            },
            Souris
        );

        modelBuilder.Entity<BiodomeEntity>().HasData(
            new BiodomeEntity
            {
                idBiodomes = 1,
                Name = "Forêt Tempérée",
                Temperature = 18,
                Humidity = 70,
                Avatar = "foret-temperee.png",
                userId = "2",
                Slug = "foret-temperee"
            },
            new BiodomeEntity
            {
                idBiodomes = 2,
                Name = "Forêt Tropicale",
                Temperature = 28,
                Humidity = 90,
                Avatar = "foret-tropicale.png",
                userId = "2",
                Slug = "foret-tropicale"
            },
            new BiodomeEntity
            {
                idBiodomes = 3,
                Name = "Savane",
                Temperature = 32,
                Humidity = 40,
                Avatar = "savane.png",
                userId = "2",
                Slug = "savane"
            },
            new BiodomeEntity
            {
                idBiodomes = 4,
                Name = "Prairie Tempérée",
                Temperature = 20,
                Humidity = 55,
                Avatar = "prairie.png",
                userId = "2",
                Slug = "prairie-temperee"
            },
            new BiodomeEntity
            {
                idBiodomes = 5,
                Name = "Laboratoire Contrôlé",
                Temperature = 22,
                Humidity = 18,
                Avatar = "labo.png",
                userId = "1",
                Slug = "laboratoire-controle"
            }
        );

        // biome prairie
        modelBuilder.Entity<BiodomeSpeciesEntity>().HasData(
            new BiodomeSpeciesEntity { BiodomeId = 4, SpeciesId = 10, Quantity = 3 }, // souris
            new BiodomeSpeciesEntity { BiodomeId = 4, SpeciesId = 9, Quantity = 10 }
        );
        // laboratoire
        modelBuilder.Entity<BiodomeSpeciesEntity>().HasData(
            new BiodomeSpeciesEntity { BiodomeId = 5, SpeciesId = 1, Quantity = 2 }, // 2 Lions
            new BiodomeSpeciesEntity { BiodomeId = 5, SpeciesId = 8, Quantity = 75 }, // 75 Lièvres
            new BiodomeSpeciesEntity { BiodomeId = 5, SpeciesId = 3, Quantity = 5 } // 5 Hêtres
        );
        // laboratoire
        modelBuilder.Entity<BiodomeSpeciesEntity>().HasData(
            new BiodomeSpeciesEntity { BiodomeId = 1, SpeciesId = 1, Quantity = 2 }, // 4 Lions
            new BiodomeSpeciesEntity { BiodomeId = 1, SpeciesId = 8, Quantity = 75 }, // 150 Lièvres
            new BiodomeSpeciesEntity { BiodomeId = 1, SpeciesId = 3, Quantity = 5 } // 10 Hêtres
        );

        modelBuilder.Entity<BiodomeHistoryEntity>().HasData(
            new BiodomeHistoryEntity
            {
                Id = 1, BiodomeId = 4, SpeciesId = 10, Delta = 5,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 2, BiodomeId = 4, SpeciesId = 10, Delta = -3,
                UpdateAt = new DateTime(2026, 4, 20, 10, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 3, BiodomeId = 4, SpeciesId = 10, Delta = 1,
                UpdateAt = new DateTime(2026, 5, 5, 9, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 4, BiodomeId = 4, SpeciesId = 9, Delta = 15,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 5, BiodomeId = 4, SpeciesId = 9, Delta = -8,
                UpdateAt = new DateTime(2026, 4, 22, 14, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 6, BiodomeId = 4, SpeciesId = 9, Delta = 3,
                UpdateAt = new DateTime(2026, 5, 3, 11, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 7, BiodomeId = 5, SpeciesId = 1, Delta = 4,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 8, BiodomeId = 5, SpeciesId = 1, Delta = -3,
                UpdateAt = new DateTime(2026, 4, 18, 9, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 9, BiodomeId = 5, SpeciesId = 1, Delta = 1,
                UpdateAt = new DateTime(2026, 5, 2, 11, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 10, BiodomeId = 5, SpeciesId = 8, Delta = 90,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 11, BiodomeId = 5, SpeciesId = 8, Delta = -25,
                UpdateAt = new DateTime(2026, 4, 25, 14, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 12, BiodomeId = 5, SpeciesId = 8, Delta = 10,
                UpdateAt = new DateTime(2026, 5, 5, 9, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 13, BiodomeId = 5, SpeciesId = 3, Delta = 8,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 14, BiodomeId = 5, SpeciesId = 3, Delta = -4,
                UpdateAt = new DateTime(2026, 4, 28, 16, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 15, BiodomeId = 5, SpeciesId = 3, Delta = 1,
                UpdateAt = new DateTime(2026, 5, 7, 10, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 16, BiodomeId = 1, SpeciesId = 1, Delta = 4,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 17, BiodomeId = 1, SpeciesId = 1, Delta = -3,
                UpdateAt = new DateTime(2026, 4, 18, 9, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 18, BiodomeId = 1, SpeciesId = 1, Delta = 1,
                UpdateAt = new DateTime(2026, 5, 2, 11, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 19, BiodomeId = 1, SpeciesId = 8, Delta = 90,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 20, BiodomeId = 1, SpeciesId = 8, Delta = -25,
                UpdateAt = new DateTime(2026, 4, 25, 14, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 21, BiodomeId = 1, SpeciesId = 8, Delta = 10,
                UpdateAt = new DateTime(2026, 5, 5, 9, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 22, BiodomeId = 1, SpeciesId = 3, Delta = 8,
                UpdateAt = new DateTime(2026, 4, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 23, BiodomeId = 1, SpeciesId = 3, Delta = -4,
                UpdateAt = new DateTime(2026, 4, 28, 16, 0, 0, DateTimeKind.Utc)
            },
            new BiodomeHistoryEntity
            {
                Id = 24, BiodomeId = 1, SpeciesId = 3, Delta = 1,
                UpdateAt = new DateTime(2026, 5, 7, 10, 0, 0, DateTimeKind.Utc)
            }
        );


        // Prodution pour équipements 
        modelBuilder.Entity<ProductionEntity>().HasData(
            new ProductionEntity
            {
                ProductionId = 1,
                Name = "Lumière"
            },
            new ProductionEntity
            {
                ProductionId = 2,
                Name = "Eau"
            },
            new ProductionEntity
            {
                ProductionId = 3,
                Name = "Azote"
            }
        );

        // Consommation pour équipements
        modelBuilder.Entity<ConsumptionEntity>().HasData(
            new ConsumptionEntity
            {
                ConsumptionId = 1,
                Name = "Hydrogène"
            },
            new ConsumptionEntity
            {
                ConsumptionId = 2,
                Name = "Electricité"
            },
            new ConsumptionEntity
            {
                ConsumptionId = 3,
                Name = "Essence"
            }
        );
        // Equipements 
        modelBuilder.Entity<EquipmentEntity>().HasData(
            new EquipmentEntity
            {
                IdEquipment = 1,
                Avatar = "lampe.webp",
                Name = "Lampe",
                ConsumptionId = 2,
                ProductionId = 1,
                Visible = true,
                UserId = "2"
            },
            new EquipmentEntity
            {
                IdEquipment = 1,
                Avatar = "gaz-extracteur.webp",
                Name = "Extracteur de gaz",
                ConsumptionId = 2,
                ProductionId = 3,
                Visible = false,
                UserId = "2"
            },
            new EquipmentEntity
            {
                IdEquipment = 1,
                Avatar = "eau-extracteur.webp",
                Name = "Extracteur d'eau",
                ConsumptionId = 3,
                ProductionId = 2,
                Visible = true,
                UserId = "2"
            }
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
            },
            new IdentityUserRole<string>
            {
                UserId = UserConfiguration.Research2Id,
                RoleId = IdentityRoleConfiguration.ResearchID
            }
        );
    }
}