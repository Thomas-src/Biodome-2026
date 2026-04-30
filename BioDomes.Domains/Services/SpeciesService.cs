using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using Microsoft.Extensions.Logging;

namespace BioDomes.Domains.Services;

public class SpeciesService : ISpeciesService
{
    private readonly ISpeciesRepository _repository;
    private readonly IPictureRepository _pictureRepository;
    private readonly ILogger<SpeciesService> _logger;

    public SpeciesService(ISpeciesRepository repository, IPictureRepository pictureRepository,
        ILogger<SpeciesService> logger)
    {
        _repository = repository;
        _pictureRepository = pictureRepository;
        _logger = logger;
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
        => species.Where(sp => sp.OwnerName.ToUpper() == ownerSpecies.ToUpper()).ToList();


    public void MakePublic(string name, string ownerName)
    {
        var species = _repository.GetAllSpecies().FirstOrDefault(s =>
            s.Nom.Equals(name, StringComparison.CurrentCultureIgnoreCase) &&
            s.OwnerName.Equals(ownerName, StringComparison.CurrentCultureIgnoreCase));

        if (species is not null)
        {
            _logger.LogInformation("found");
            species.IsVisible = true;

            _repository.Update(name, ownerName, species);
        }
        else
        {
            _logger.LogInformation("not found");
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

    public bool SpeciesExists(Species species) => _repository.GetAllSpecies().Any(e =>
        e.Nom.Equals(species.Nom, StringComparison.OrdinalIgnoreCase) &&
        (e.IsVisible || e.OwnerName == species.OwnerName));

    public Species? GetByKebabCase(string kebabCase, string ownerName)
    {
        if (string.IsNullOrWhiteSpace(kebabCase) || string.IsNullOrWhiteSpace(ownerName))
        {
            return null;
        }

        return _repository.GetAllSpecies().FirstOrDefault(s =>
            s.Nom.ToKebabCase().Equals(kebabCase, StringComparison.CurrentCultureIgnoreCase) &&
            s.OwnerName.Equals(ownerName, StringComparison.CurrentCultureIgnoreCase));
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
        }
        catch (Exception e)
        {
            Console.WriteLine("==============\nEXCPETION : " + e + "==============");
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
            if (openReadStream != null)
            {
                if (!string.IsNullOrEmpty(oldSpecies.Avatar))
                {
                    _pictureRepository.Delete(oldSpecies.Avatar, "species");
                }

                var filename = _pictureRepository.Save(
                    oldSpecies.Avatar,
                    openReadStream,
                    "species"
                );
                newSpecies.Avatar = filename;
            }

            _repository.Update(oldSpecies.Nom, oldSpecies.OwnerName, newSpecies);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("============== UPDATE EXCEPTION ==============");
            Console.WriteLine(ex);
            return false;
        }
    }

    public void Delete(string name, string ownerName)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ownerName))
        {
            return;
        }

        var species = GetByKebabCase(name, ownerName);

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