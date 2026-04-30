using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface IResearchService
{
    IReadOnlyList<ResearchOrganism> GetAllOrganisms();
}