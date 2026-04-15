using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;

public class UserConfiguration  : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property<string>(user => user.Email).IsRequired().HasMaxLength(128);
        builder.Property<string>(user => user.UserName).IsRequired().HasMaxLength(128);
       

        SeedUsers(builder);
    }
    public static readonly string AdminId =  Environment.MachineName+DateTime.Parse("2024-04-01T10:30:00.0")+"A";
    public static readonly string ResearchId =  Environment.MachineName+DateTime.Parse("2024-04-01T10:30:00.0")+"B";

    private void SeedUsers(EntityTypeBuilder<User> builder)
    {
        PasswordHasher<User> ph = new PasswordHasher<User>();
        
        var admin = new User()
        {
            Id = AdminId,
            Email = "admin@company.com",
            EmailConfirmed = true, 
            UserName = "Admin",
            NormalizedUserName = "Admin".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2000,1,1))
        };
        admin.PasswordHash = ph.HashPassword(admin, "Test1234$"); // todo mettre mdp, email et nom dans la config
        
        var research = new User()
        {
            Id = ResearchId,
            Email = "research@company.com",
            EmailConfirmed = true, 
            UserName = "Chercheur",
            NormalizedUserName = "Chercheur".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2000,1,1))
        };
        admin.PasswordHash = ph.HashPassword(admin, "Test1234$"); // todo mettre mdp, email et nom dans la config

        builder.HasData(admin,research);
    }

}