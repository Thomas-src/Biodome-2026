using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;

namespace BioDomes.Infrastructures.repository;

public class SpeciesClassificationRepository(BioDomesDbContext context) : ISpeciesClassificationRepository
{

    public IReadOnlyList<SpeciesClassification> GetAllSpeciesClassifications()
    {
        return context.SpeciesTypes.Select(st => Enum.Parse<SpeciesClassification>(st.Name)).Distinct().ToList();
    }
}