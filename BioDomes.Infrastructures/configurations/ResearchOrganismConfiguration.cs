using BioDomes.Infrastructures.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.configurations;

public class ResearchOrganismConfiguration : IEntityTypeConfiguration<ResearchOrganismEntity>
{
    public void Configure(EntityTypeBuilder<ResearchOrganismEntity> builder)
    {
        builder.HasData(
            new ResearchOrganismEntity { ResearchOrganismId = "1", Name = "ULiege" },
            new ResearchOrganismEntity { ResearchOrganismId = "2", Name = "UCLouvain" },
            new ResearchOrganismEntity { ResearchOrganismId = "3", Name = "HELMo" },
            new ResearchOrganismEntity { ResearchOrganismId = "4", Name = "Hec Liège" },
            new ResearchOrganismEntity { ResearchOrganismId = "5", Name = "Hénallux" },
            new ResearchOrganismEntity { ResearchOrganismId = "6", Name = "HEPL" }
        );
    }
}