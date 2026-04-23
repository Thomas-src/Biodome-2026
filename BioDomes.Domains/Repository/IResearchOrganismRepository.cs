using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IResearchOrganismRepository
{
    IReadOnlyList<ResearchOrganism> GetAllOrganisms();
}