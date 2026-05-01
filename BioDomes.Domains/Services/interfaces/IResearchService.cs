using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IResearchService
{
    IReadOnlyList<ResearchOrganism> GetAllOrganisms();
}