using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Domains.Utils;
using Microsoft.Extensions.Logging;

namespace BioDomes.Domains.Services;

public class BiodomeService : IBiodomeService
{
    private readonly IBiodomeRepository _repository;
    private readonly IPictureRepository _pictureRepository;
    private readonly ILogger<BiodomeService> _logger;

    public BiodomeService(IBiodomeRepository repository, IPictureRepository pictureRepository,
        ILogger<BiodomeService> logger)
    {
        _repository = repository;
        _pictureRepository = pictureRepository;
        _logger = logger;
    }

    public IReadOnlyList<Biodome> GetBestThreeBiodome()
        => _repository.GetAllBiodomes()
            .OrderByDescending(b => b.Etat)
            .ThenByDescending(b => b.Species.Count)
            .Take(3)
            .ToList();

    public IReadOnlyList<Biodome> GetAllBiodomes() => _repository.GetAllBiodomes();

    public IReadOnlyList<Biodome> HasSpecies(string speciesName, string ownerName) => _repository.GetAllBiodomes()
        .Where(b => b.Species.Any(s =>
            s.Nom.ToLower().Equals(speciesName.ToLower()) && s.OwnerName.ToLower().Equals(ownerName.ToLower())))
        .ToList();


    public IReadOnlyList<Biodome> HasEquipement(string equipementName, string ownerName) => _repository.GetAllBiodomes()
        .Where(b => b.Equipements.Any(e =>
            e.Name.ToLower().Equals(equipementName.ToLower()) && e.OwnerName.ToLower().Equals(ownerName.ToLower())))
        .ToList();


    public IReadOnlyList<Biodome> GetUserBiodomes(string username)
    {
        var biodomes = _repository.GetAllBiodomes();

        username = username.ToLowerInvariant();

        return biodomes.Where(biodome =>
                !string.IsNullOrWhiteSpace(biodome.CreatorName) &&
                biodome.CreatorName.Equals(username, StringComparison.OrdinalIgnoreCase))
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
        {
            query = query.Where(b => b.Temperature >= temperatureMin.Value);
        }

        if (temperatureMax.HasValue)
        {
            query = query.Where(b => b.Temperature <= temperatureMax.Value);
        }


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
            query =
                query.Where(b =>
                    b.Equipements.Any(e => e.Name.Contains(equipement, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrWhiteSpace(espece))
        {
            query = query.Where(b => b.Species.Any(s => s.Nom.Contains(espece, StringComparison.OrdinalIgnoreCase)));
        }

        return query.ToList();
    }

    public double CalculateHumiditeMax(double temp) => HumidityCalculator.MaxHumidityOf(temp);

    public Biodome? GetBiodomeById(int biodomeId) => GetAllBiodomes().FirstOrDefault(b => b.Id == biodomeId);


    public void AddSpeciesTo(int biomeId, string speciesName, int quantity) =>
        _repository.AddSpeciesInBiome(biomeId, speciesName, quantity);

    public void AddEquipementTo(int biomeId, string equipementName) =>
        _repository.AddEquipementInBiome(biomeId, equipementName);

    public void RemoveSpeciesFrom(int biomeId, string speciesName) =>
        _repository.RemoveSpeciesInBiome(biomeId, speciesName);

    public void RemoveEquipementFrom(int biomeId, string equipementName) =>
        _repository.RemoveEquipementInBiome(biomeId, equipementName);

    public void Update(Biodome created)
    {
        var old = GetBiodomeById(created.Id);

        if (old != null && !created.DeepEquals(old))
        {
            _repository.Update(created);
        }
    }

    public void Update(Biodome created, string fileName, Stream openReadStream)
    {
        var old = GetBiodomeById(created.Id);

        if (old != null && !created.DeepEquals(old))
        {
            created.Image = _pictureRepository.Save(fileName, openReadStream, "biodomes");
            _repository.Update(created);
        }
    }
}