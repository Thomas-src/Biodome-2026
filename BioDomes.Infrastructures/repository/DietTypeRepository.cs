using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class DietTypeRepository : IDietTypeRepository
{
    private readonly BioDomesDbContext _context;

    public DietTypeRepository(BioDomesDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<DietType> GetAllDietTypes()
    {
        return _context.SpeciesRegimes
            .Select(sr => Enum.Parse<DietType>(sr.Name))
            .Distinct()
            .ToList();
    }
}