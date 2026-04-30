using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface IBiodomeService
{
    IReadOnlyList<Biodome> GetBestThreeBiodome();
    IReadOnlyList<Biodome> GetAllBiodomes();
    IReadOnlyList<Biodome> HasSpecies(string speciesName, string ownerName);
    IReadOnlyList<Biodome> HasEquipement(string equipementName, string ownerName);
    IReadOnlyList<Biodome> GetUserBiodomes(string username);

    IReadOnlyList<Biodome> SearchBiodomes(double? temperatureMin, double? temperatureMax, double? humiditeMin,
        double? humiditeMax, string? equipement, string? espece);

    void Save(Biodome created, Stream openReadStream);

    double CalculateHumiditeMax(double temp);
    Biodome GetBiodomeById(int biodomeId);
    void Update(Biodome created);
    void Update(Biodome created, string fileName, Stream openReadStream);
    void AddSpeciesTo(int biomeId, string speciesName, int quantity);
    void AddEquipementTo(int biomeId, string equipementName);
    void RemoveSpeciesFrom(int biomeId, string speciesName);
    void RemoveEquipementFrom(int biomeId, string equipementName);
}