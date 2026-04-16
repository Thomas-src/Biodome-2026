using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;


public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public static readonly string AdminId = Environment.MachineName+DateTime.Parse("2024-04-01T10:30:00.0")+"Admin";
    public static readonly string ResearchID =  Environment.MachineName+DateTime.Parse("2024-04-01T10:30:00.0")+"CeoId";
    
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        var adminRole = new IdentityRole()
        {
            Id = AdminId, ConcurrencyStamp = AdminId, Name = "Admin", NormalizedName = "ADMIN", 
        };
        
        var researchRole = new IdentityRole()
        {
            Id = ResearchID, ConcurrencyStamp = ResearchID, Name = "Chercheur", NormalizedName = "CHERCHEUR"
        };

        builder.HasData(adminRole, researchRole);
    }
}