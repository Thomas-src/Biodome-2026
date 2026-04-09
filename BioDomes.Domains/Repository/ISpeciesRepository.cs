using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    IReadOnlyList<SpeciesClassification> GetAllSpeiesTypes();
}