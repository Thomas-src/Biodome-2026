using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Utils;

namespace BioDomes.Domains.Services;

public class BiodomeService : IBiodomeService
{
    private IBiodomeRepository _repository;
    private IPictureRepository _pictureRepository;

    public BiodomeService(IBiodomeRepository repository, IPictureRepository pictureRepository)
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

    public IReadOnlyList<Biodome> HasSpecies(string speciesName)
    {
        var biodomes = _repository.GetAllBiodomes();

        return biodomes
            .Where(b => b.Species.Any(s => s.Nom.Equals(speciesName)))
            .ToList();
    }

    public IReadOnlyList<Biodome> HasEquipement(string equipementName)
    {
        var biodomes = _repository.GetAllBiodomes();

        return biodomes
            .Where(b => b.Equipements.Any(s => s.Name.Equals(equipementName)))
            .ToList();
    }

    public void Save(Biodome created, Stream openReadStream)
    {
        // 1. Enregistrer l'image
        created.Image = _pictureRepository.Save(created.Image, openReadStream, "biodomes");

        //2. Enregistrer le biome
        _repository.Create(created);
    }

    public IReadOnlyList<Biodome> SearchBiodomes(double? temperatureMin, double? temperatureMax, double? humiditeMin,
        double? humiditeMax, string? equipement, string? espece)
    {
        var query = _repository.GetAllBiodomes().AsQueryable();

        if (temperatureMin.HasValue)
            query = query.Where(b => b.Temperature >= temperatureMin.Value);

        if (temperatureMax.HasValue)
            query = query.Where(b => b.Temperature <= temperatureMax.Value);

        if (humiditeMin.HasValue)
        {
            query = query.Where(b => b.Humidite >= humiditeMin.Value);
        }

        if (humiditeMax.HasValue)
        {
            query = query.Where(b => b.Humidite <= humiditeMax.Value);
        }


        if (!string.IsNullOrWhiteSpace(equipement))
        {
            //  query = query.Where(b => b.Equipements.Any(e => e.Nom.Contains(equipement, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrWhiteSpace(espece))
        {
            query = query.Where(b => b.Species.Any(s => s.Nom.Contains(espece, StringComparison.OrdinalIgnoreCase)));
        }

        return query.ToList();
    }
    
    public double CalculateHumiditeMax(double temp)
    {
        return HumidityCalculator.MaxHumidityOf(temp);
    }

    public Biodome GetBiodomeById(int biodomeId)
    {
        return _repository.GetBiodomeById(biodomeId);
    }

    public void AddSpecyTo(int biomeId, string speciesName, int quantity)
    {
        _repository.AddSpecyInBiome(biomeId, speciesName, quantity);
    }

    public void AddEquipementTo(int biomeId, string equipementName)
    {
        _repository.AddEquipementInBiome(biomeId, equipementName);
    }

    public void RemoveSpecyFrom(int biomeId, string speciesName)
    {
        _repository.RemoveSpecyInBiome(biomeId, speciesName);
    }

    public void RemoveEquipementFrom(int biomeId, string equipementName)
    {
        _repository.RemoveEquipementInBiome(biomeId, equipementName);

    }
    
    public void Update(Biodome created)
    {
        var old = _repository.GetBiodomeById(created.Id);

        if (!created.DeepEquals(old))
        {
            _repository.Update(created);
        }
    }

    public void Update(Biodome created, string fileName, Stream openReadStream)
    {
        var old = _repository.GetBiodomeById(created.Id);

        if (!created.DeepEquals(old))
        {
            created.Image = _pictureRepository.Save(fileName, openReadStream, "biodomes");
            
            _repository.Update(created);
        }
    }
}