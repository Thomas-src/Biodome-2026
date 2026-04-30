using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
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

    public Species? GetSpeciesBySlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }
        slug = slug.ToLowerInvariant();

        foreach (var species in _repository.GetAllSpecies())
        {
            var speciesSlug = species.Nom.ToKebabCase();

            if (speciesSlug.Equals(slug))
            {
                return species;
            }
        }

        return null;
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

    public Species? Get(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }
        name = name.ToLowerInvariant();
        foreach (var specy in _repository.GetAllSpecies())
        {
            if (specy.Nom.ToLowerInvariant().Equals(name))
            {
                return specy;
            }
        }
        return null;
    }

    public bool Save(Species created, Stream? openReadStream)
    {
        try
        {
            if (openReadStream != null)
            {
                created.Avatar = _pictureRepository.Save(created.Avatar, openReadStream, "species");
            }
             
            _repository.Save(created);
        } catch (Exception e)
        {
            Console.WriteLine("==============\nEXCPETION : " +e + "==============");
            return false;
        }
        return true;
    }

    public bool Update(Species oldSpecies, Species newSpecies, Stream? openReadStream)
    {
        try
        {
            if (oldSpecies == null || newSpecies == null)
            {
                return false;
            }
            oldSpecies.IsVisible = newSpecies.IsVisible;
    
            // ⚠️ ici tu es limité par ton modèle (pas de setters pour Nom/Taille/Poids/DietType)
            // donc normalement tu dois soit :
            // - ajouter des méthodes métier dans Species
            // - soit accepter que seul avatar + visibility changent
    
            // 2. Image update (si nouvelle image fournie)
            if (openReadStream != null)
            {
                if (!string.IsNullOrEmpty(oldSpecies.Avatar))
                {
                    _pictureRepository.Delete(oldSpecies.Avatar, "species");
                }
    
                oldSpecies.Avatar = _pictureRepository.Save(
                    oldSpecies.Avatar,
                    openReadStream,
                    "species"
                );
            }
    
            _repository.Update(oldSpecies.Nom, newSpecies);
    
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("============== UPDATE EXCEPTION ==============");
            Console.WriteLine(ex);
            return false;
        }
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