using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;

namespace BioDomes.Infrastructures.repository;

public class DietTypeRepository(BioDomesDbContext context) : IDietTypeRepository
{
    public IReadOnlyList<DietType> GetAllDietTypes()
    {
        return context.SpeciesRegimes
            .Select(sr => Enum.Parse<DietType>(sr.Name))
            .Distinct()
            .ToList();
    }
}