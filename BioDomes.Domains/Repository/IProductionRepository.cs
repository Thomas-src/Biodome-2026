using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Repository;

public interface IProductionRepository
{
    IReadOnlyList<Production> GetAllProductions();
}