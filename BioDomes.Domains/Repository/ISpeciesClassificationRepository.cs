using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpeciesClassificationRepository
{
    IReadOnlyList<SpeciesClassification> GetAllSpeciesClassifications();
}