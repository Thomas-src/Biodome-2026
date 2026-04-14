using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;

public class UserConfiguration  : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        throw new NotImplementedException();
    }
    public static readonly string AdminId =  Environment.MachineName+DateTime.Parse("2024-04-01T10:30:00.0")+"A";
    private void SeedUsers(EntityTypeBuilder<User> builder)
    {
        PasswordHasher<User> ph = new PasswordHasher<User>();
        
        var admin = new User()
        {
            Id = AdminId,
            Email = "admin@company.com",
            EmailConfirmed = true, 
            UserName = "admin@company.com",
            NormalizedUserName = "alice@admin.com".ToUpper(),
        };
        admin.PasswordHash = ph.HashPassword(admin, "Test1234$"); // todo mettre mdp, email et nom dans la config
        
        builder.HasData(admin);
    }

}