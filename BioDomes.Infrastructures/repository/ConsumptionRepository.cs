using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class ConsumptionRepository(BioDomesDbContext context) : IConsumptionRepository
{
    public IReadOnlyList<Consumptions> GetAllConsumptions()
    {
        return context.Consumptions.Select(c => Enum.Parse<Consumptions>(c.Name)).ToList();
    }
}