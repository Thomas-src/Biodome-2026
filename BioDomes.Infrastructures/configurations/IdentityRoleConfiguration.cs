using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;


public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public static readonly string AdminId = "0";
    public static readonly string ResearchID =  "1";
    
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