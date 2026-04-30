using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    IReadOnlyList<Species> GetAllSpecies();
    void Save(Species created);
    void Update(string name, string ownerName, Species species);
    void Delete(string specyNom);
}