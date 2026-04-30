using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface ISpeciesService
{
    public Species? GetSpeciesBySlug(string slug);
    public IReadOnlyList<Species> FilterOwnerSpecies(string ownerSpecies, IReadOnlyList<Species> species);
    public void MakePublic(string name);
    public IReadOnlyList<Species> FilterSpecies(string ownerSpecies, IReadOnlyList<Species> species);
    public bool SpeciesExists(Species species);
    
    public IReadOnlyList<Species> GetAvailableSpecies();
    public IReadOnlyList<Species> GetUnavailableSpecies();
    public Species? Get(string name);
    public void Delete(string name);
    bool Save(Species created, Stream openReadStream);
    public bool Update(Species oldSpecies, Species newSpecies, Stream openReadStream);
}