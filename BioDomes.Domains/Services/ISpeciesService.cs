using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface ISpeciesService
{
    public IReadOnlyList<Species> FilterOwnerSpecies(string ownerSpecies, IReadOnlyList<Species> species);

    public IReadOnlyList<Species> FilterSpecies(string ownerSpecies, IReadOnlyList<Species> species);
    
    public IReadOnlyList<Species> GetAvailableSpecies();
    public IReadOnlyList<Species> GetUnavailableSpecies();
    public bool SpeciesExists(Species species);
    bool Save(Species created, Stream openReadStream);

}