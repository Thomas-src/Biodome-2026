using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Repository;

public interface IConsumptionRepository
{
    public IReadOnlyList<Consumptions> GetAllConsumptions();
}