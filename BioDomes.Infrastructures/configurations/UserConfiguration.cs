using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;

public class UserConfiguration  : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property<string>(user => user.Email).IsRequired().HasMaxLength(128);
        builder.Property<string>(user => user.UserName).IsRequired().HasMaxLength(128);
       

        SeedUsers(builder);
    }
    public static readonly string AdminId =  "1";
    public static readonly string ResearchId =  "2";

    private void SeedUsers(EntityTypeBuilder<UserEntity> builder)
    {
        PasswordHasher<UserEntity> ph = new PasswordHasher<UserEntity>();
        
        var admin = new UserEntity()
        {
            Id = AdminId,
            Email = "admin@company.com",
            EmailConfirmed = true, 
            UserName = "Admin",
            NormalizedUserName = "Admin".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2000,1,1)),
            ResearchOrganismId = "1",
            Avatar = "3.png"
        };
        admin.PasswordHash = ph.HashPassword(admin, "Test1234$");
        
        var research = new UserEntity()
        {
            Id = ResearchId,
            Email = "research@company.com",
            EmailConfirmed = true, 
            UserName = "Chercheur",
            NormalizedUserName = "Chercheur".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2000,1,1)),
            ResearchOrganismId = "1",
            Avatar = "1.png"
        };
        admin.PasswordHash = ph.HashPassword(research, "Test1234$");

        var research2 = new UserEntity()
        {
            Id = "3",
            Email = "research@helmo.be",
            EmailConfirmed = true, 
            UserName = "Chercheur 2",
            NormalizedUserName = "Chercheur 2".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2001,12,01)),
            ResearchOrganismId = "3",
            Avatar = "4.png"
        };
        research2.PasswordHash = ph.HashPassword(admin, "Helmo1234$");

        
        builder.HasData(admin,research);
    }

}