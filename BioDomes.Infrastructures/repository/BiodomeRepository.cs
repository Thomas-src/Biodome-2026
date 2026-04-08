using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class BiodomeRepository : IBiodomeRepository
{
    
    private readonly BioDomesDbContext _context;
    
    public BiodomeRepository(BioDomesDbContext context)
    {
        _context = context;
    }
    
    public IReadOnlyList<Biodome> GetBestThreeBiodome()
    {
        return _context.Biodomes.Select(b => new Biodome
            (b.idBiodomes, b.Name, b.Temperature,
                b.Humidity, b.Avatar))
            .ToList<Biodome>();

    }

    public IReadOnlyList<Biodome> GetAllBiodomes()
    {
        return _context.Biodomes.Select(b => new Biodome
            (b.idBiodomes, b.Name, b.Temperature,
                b.Humidity, b.Avatar))
            .ToList<Biodome>();
    }
}