using BioDomes.Infrastructures.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;

public class ResearchOrganismConfiguration : IEntityTypeConfiguration<ResearchOrganism>
{
    public void Configure(EntityTypeBuilder<ResearchOrganism> builder)
    {
        builder.HasData(
            new ResearchOrganism { ResearchOrganismId = "1", Name = "ULiege" },
            new ResearchOrganism { ResearchOrganismId = "2", Name = "UCLouvain" },
            new ResearchOrganism { ResearchOrganismId = "3", Name = "HELMo" }
        );
    }
    
}