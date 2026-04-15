using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.repository;

namespace BioDomes.Infrastructures.Repository;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly BioDomesDbContext _context;

    public SpeciesRepository(BioDomesDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Species> GetAllSpecies()
    {
        return _context.Species.Select(s => new Species(s.Name,
            s.Size,
            s.Weight,
            Enum.Parse<DietType>(s.Regim.Name),
            Enum.Parse<SpeciesClassification>(s.Type.Name),
            s.Avatar)
        ).ToList();
    }
}