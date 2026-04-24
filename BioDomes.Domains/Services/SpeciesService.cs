using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class SpeciesService : ISpeciesService
{
    private readonly ISpeciesRepository _repository;

    public SpeciesService(ISpeciesRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Species> FilterOwnerSpecies(string ownerSpecies, IReadOnlyList<Species> species)
    {
        IList<Species> result = new List<Species>();

        foreach (var sp in species)
        {
            if (sp.OwnerName != null)
            {
                if (sp.OwnerName.ToUpper() == ownerSpecies.ToUpper())
                {
                    result.Add(sp);
                }
            }
        }

        return result.AsReadOnly();
    }

    public IReadOnlyList<Species> FilterSpecies(string ownerSpecies, IReadOnlyList<Species> species)
    {
        IList<Species> result = new List<Species>();

        foreach (var sp in species)
        {
            if (sp.IsVisible)
            {
                if (sp.OwnerName == null)
                {
                    result.Add(sp);
                }
                else if (sp.OwnerName.ToUpper() != ownerSpecies.ToUpper())
                {
                    result.Add(sp);
                }
            }
        }

        return result.AsReadOnly();
    }

    public bool SpeciesExists(Species species)
    {
        foreach (var specy in _repository.GetAllSpecies())
        {
            if (species.Equals(specy))
            {
                return true;
            }
        }

        return false;
    }

    public bool Save(Species created)
    {
        try
        {
            _repository.Save(created);
        } catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
}