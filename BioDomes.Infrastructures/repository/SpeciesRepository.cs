using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repository;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly BioDomesDbContext _context;

    public SpeciesRepository(BioDomesDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<SpeciesClassification> GetAllSpeiesTypes()
    {
        return null;
        // return _context.SpeciesTypes.Select(
        //     s => s.Name);
    }
}