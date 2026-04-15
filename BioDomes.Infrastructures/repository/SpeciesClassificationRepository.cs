using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;

namespace BioDomes.Infrastructures.repository;

public class SpeciesClassificationRepository : ISpeciesClassificationRepository
{
    private readonly BioDomesDbContext _context;

    public SpeciesClassificationRepository(BioDomesDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<SpeciesClassification> GetAllSpeciesClassifications()
    {
        return _context.SpeciesTypes.Select(st => Enum.Parse<SpeciesClassification>(st.Name)).Distinct().ToList();
    }
}