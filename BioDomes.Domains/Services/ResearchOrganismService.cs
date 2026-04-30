using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class ResearchOrganismService(IResearchOrganismRepository repository) : IResearchService
{
    public IReadOnlyList<ResearchOrganism> GetAllOrganisms() => repository.GetAllOrganisms();
}