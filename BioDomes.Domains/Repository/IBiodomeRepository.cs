using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiodomeRepository
{
    IReadOnlyList<Biodome> GetAllBiodomes();

    void Create(Biodome biodome);
    void Update(string oldSlug, Biodome biodome);
    void AddSpeciesInBiome(string biodomeSlug, string speciesName, int quantity);
    void AddEquipementInBiome(string biodomeSlug, string equipementName);
    void RemoveSpeciesInBiome(string biodomeSlug, string speciesName);
    void RemoveEquipementInBiome(string biodomeSlug, string equipementName);
    Biodome? FindBiodome(string biodomeSlug);
    void DeleteBiodome(string biodomeSlug);
}