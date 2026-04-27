using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class ProductionRepository : IProductionRepository
{
    private readonly BioDomesDbContext _context;
    
    public ProductionRepository(BioDomesDbContext context)
    {
        _context = context;
    }
    
    public IReadOnlyList<Production> GetAllProductions()
    {
        return _context.productions.Select(p => Enum.Parse<Production>(p.Name)).ToList();
    }
}