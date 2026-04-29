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

    public void MakePublic(string name)
    {
        var species = _repository.GetSpecies(name);

        if (species is not null)
        {
            species.IsVisible = true;

            _repository.Update(name, species);
        }
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

    public Species Get(string name)
    {
        foreach (var specy in _repository.GetAllSpecies())
        {
            if (specy.Nom.Equals(name))
            {
                return specy;
            }
        }
        return null;
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

    public void Delete(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }
        var species = _repository.GetSpecies(name);

        if (species is null)
        {
            return;
        }
        
        if (!string.IsNullOrEmpty(species.Avatar))
        {
            _pictureRepository.Delete(species.Avatar, "species");
        }

        _repository.Delete(species.Nom);
    }
}