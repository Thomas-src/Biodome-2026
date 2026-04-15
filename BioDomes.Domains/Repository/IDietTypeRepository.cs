using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IDietTypeRepository
{
    IReadOnlyList<DietType> GetAllDietTypes();
}