using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class BiodomeService : IBiodomeService
{
    private IBiodomeRepository _repository;
    private IPictureRepository _pictureRepository;
    public BiodomeService(IBiodomeRepository repository,  IPictureRepository pictureRepository)
    {
        _repository = repository;
        _pictureRepository = pictureRepository;
    }
    public IReadOnlyList<Biodome> GetBestThreeBiodome()
        => _repository.GetAllBiodomes()
            .OrderByDescending(b => b.Etat)
            .ThenByDescending(b => b.Species.Count)
            .Take(3)
            .ToList();

    public IReadOnlyList<Biodome> GetAllBiodomes()
    => _repository.GetAllBiodomes();

    public void Save(Biodome created, Stream openReadStream)
    {
        // 1. Enregistrer l'image
        created.Image = _pictureRepository.Save(created.Image, openReadStream, "biodomes");
        
        //2. Enregistrer le biome
        _repository.Create(created);
    }

    public double CalculateHumiditeMax(double temp)
    {
        return (216.7 * 6.112 * Math.Exp((17.67 * temp) / (temp + 243.5)))
               / (temp + 273.15);
    }
}