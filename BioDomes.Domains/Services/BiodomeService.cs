using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class BiodomeService : IBiodomeService
{
    private IBiodomeRepository _repository;
    public BiodomeService(IBiodomeRepository repository)
    {
        _repository = repository;
    }
    public IReadOnlyList<Biodome> GetBestThreeBiodome()  // todo nb espèces le plus haut
        => _repository.GetAllBiodomes().OrderByDescending(b => b.Etat).Take(3).ToList();
    

    public IReadOnlyList<Biodome> GetAllBiodomes()
    => _repository.GetAllBiodomes();

    // TODO deleguer dans un repo commun
    public void SavePicture(Biodome created, string fileName, Stream fileStream)
    {
        var newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "biodomes");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            fileStream.CopyTo(stream);
        }

        created.Image = "/images/species/" + newFileName;
    }

    public void Save(Biodome created)
    {
        _repository.Create(created);
    }
}