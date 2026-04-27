using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class ConsumptionRepository : IConsumptionRepository
{
    private BioDomesDbContext _context;

    public ConsumptionRepository(BioDomesDbContext context)
    {
        _context = context;
    }
    
    public IReadOnlyList<Consumptions> GetAllConsumptions()
    {
        return _context.consumptions.Select(c => Enum.Parse<Consumptions>(c.Name)).ToList();
    }
}