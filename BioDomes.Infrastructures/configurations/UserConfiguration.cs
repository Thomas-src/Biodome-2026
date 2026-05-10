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
    public static readonly string Research2Id =  "3";

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
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2001,11,1)),
            ResearchOrganismId = "1",
            Avatar = "3.png"
        };
        admin.PasswordHash = ph.HashPassword(admin, "Test1234$");
        
        var chercheurUliege = new UserEntity()
        {
            Id = ResearchId,
            Email = "research@ulg.be",
            EmailConfirmed = true, 
            UserName = "Darwin",
            NormalizedUserName = "Darwin".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2002,03,12)),
            ResearchOrganismId = "1",
            Avatar = "1.png"
        };
        chercheurUliege.PasswordHash = ph.HashPassword(chercheurUliege, "Dwin1859$$");

        var chercheurHelmo = new UserEntity()
        {
            Id = Research2Id,
            Email = "research@helmo.be",
            EmailConfirmed = true, 
            UserName = "JohnDoe",
            NormalizedUserName = "JohnDoe".ToUpper(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2001,12,21)),
            ResearchOrganismId = "3",
            Avatar = "4.png"
        };
        chercheurHelmo.PasswordHash = ph.HashPassword(chercheurHelmo, "DoeJ1234!");

        
        builder.HasData(admin,chercheurUliege,chercheurHelmo);
    }

}