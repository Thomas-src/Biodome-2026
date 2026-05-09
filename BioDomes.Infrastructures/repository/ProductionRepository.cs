using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class ProductionRepository(BioDomesDbContext context) : IProductionRepository
{
    public IReadOnlyList<Production> GetAllProductions()
    {
        return context.Productions.Select(p => Enum.Parse<Production>(p.Name)).ToList();
    }
}