using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface ISpeciesService
{
    
    public IReadOnlyList<Species> FilterOwnerSpecies(string ownerSpecies, IReadOnlyList<Species> species);
    public void MakePublic(string name, string ownerName);
    public IReadOnlyList<Species> FilterSpecies(string ownerSpecies, IReadOnlyList<Species> species);
    public bool SpeciesExists(Species species);
    
    public IReadOnlyList<Species> GetAvailableSpecies();
    public IReadOnlyList<Species> GetUnavailableSpecies();
    Species? GetByKebabCase(string kebabCase, string ownerName);
    public void Delete(string name, string ownerName);
    bool Save(Species created, Stream openReadStream);
    public bool Update(Species oldSpecies, Species newSpecies, Stream openReadStream);
}