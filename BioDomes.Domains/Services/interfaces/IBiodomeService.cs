using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IBiodomeService
{
    
    IReadOnlyList<Biodome> GetBestThreeBiodome();
    IReadOnlyList<Biodome> GetAllStableBiodomes();
    IReadOnlyList<Biodome> HasSpecies(string speciesName, string ownerName);
    IReadOnlyList<Biodome> HasEquipement(string equipementName, string ownerName);
    IReadOnlyList<Biodome> GetUserBiodomes(string username);

    IReadOnlyList<Biodome> SearchBiodomes(double? temperatureMin, double? temperatureMax, double? humiditeMin,
        double? humiditeMax, string? equipement, string? espece);

    void Save(Biodome created, Stream openReadStream);

    Biodome? GetBiodomeBySlug(string biodomeSlug);
    void Update(Biodome created);
    void Update(Biodome created, string fileName, Stream openReadStream);
    void AddSpeciesTo(string biodomeSlug, string speciesName, int quantity);
    void AddEquipementTo(string biodomeSlug, string equipementName);
    void RemoveSpeciesFrom(string biodomeSlug, string speciesName);
    void RemoveEquipementFrom(string biodomeSlug, string equipementName);
    void DeleteBiodome(string biodomeSlug);
    bool BiodomeExists(string biodomeSlug);
}