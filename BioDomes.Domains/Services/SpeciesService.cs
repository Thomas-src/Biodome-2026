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
            Console.WriteLine("==============\nEXCPETION : " +e + "==============");
            return false;
        }
        return true;
    }

    public void SavePicture(Species created, string fileName, Stream fileStream)
    {
        var newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "species");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            fileStream.CopyTo(stream);
        }

        created.Avatar = newFileName;
    }
}