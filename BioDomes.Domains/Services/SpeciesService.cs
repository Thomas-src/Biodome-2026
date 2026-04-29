using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class SpeciesService : ISpeciesService
{
    private readonly ISpeciesRepository _repository;
    private readonly IPictureRepository _pictureRepository;

    public SpeciesService(ISpeciesRepository repository, IPictureRepository pictureRepository)
    {
        _repository = repository;
        _pictureRepository = pictureRepository;
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
    

    public IReadOnlyList<Species> GetUnavailableSpecies()
    {
        return _repository.GetAllSpecies()
            .Where(s => !s.IsVisible)
            .ToList();
    }
    public IReadOnlyList<Species> GetAvailableSpecies()
    {
        return _repository.GetAllSpecies()
            .Where(s => s.IsVisible)
            .ToList();
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

    public bool Save(Species created, Stream openReadStream)
    {
        try
        {
            created.Avatar = _pictureRepository.Save(created.Avatar, openReadStream, "species");
            
            _repository.Save(created);
        } catch (Exception e)
        {
            Console.WriteLine("==============\nEXCPETION : " +e + "==============");
            return false;
        }
        return true;
    }
}