using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class ResearchOrganismRepository(BioDomesDbContext context) : IResearchOrganismRepository
{
    public IReadOnlyList<ResearchOrganism> GetAll()
        => context.ResearchOrganisms
            .Select(r => new ResearchOrganism(r.ResearchOrganismId, r.Name))
            .ToList();
}