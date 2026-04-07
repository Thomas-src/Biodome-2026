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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // 🔑 Clés primaires
        // =========================
        modelBuilder.Entity<Biodomes>()
            .HasKey(b => b.idBiodomes);

        modelBuilder.Entity<Species>()
            .HasKey(s => s.IdSpecies);

        modelBuilder.Entity<Equipment>()
            .HasKey(e => e.IdEquipment);

        modelBuilder.Entity<ResearchOrganism>()
            .HasKey(r => r.IdOrgansim);

        // =========================
        // 🔗 User → Biodomes (1-N)
        // =========================
        modelBuilder.Entity<Biodomes>()
            .HasOne(b => b.user)
            .WithMany(u => u.Biodomes)
            .HasForeignKey(b => b.userId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // 🔗 User → Equipment (1-N)
        // =========================
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.User)
            .WithMany(u => u.Equipments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // =========================
        // 🔗 User → Species (1-N)
        // =========================
        modelBuilder.Entity<Species>()
            .HasOne(s => s.User)
            .WithMany(u => u.Species)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // =========================
        // 🔗 ResearchOrganism → User (1-N)
        // Plusieurs Users peuvent partager le même organisme
        // =========================
        modelBuilder.Entity<User>()
            .HasOne(u => u.ResearchOrganism)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.ResearchOrganismeId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // 🔗 Biodomes ↔ Species (N-N)
        // =========================
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

        // =========================
        // 🔗 Biodomes ↔ Equipment (N-N)
        // =========================
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
    }
}