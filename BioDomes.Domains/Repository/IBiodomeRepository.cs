using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiodomeRepository
{
    IReadOnlyList<Biodome> GetAllBiodomes();
    
    void Create(Biodome biodome);
    void Update(Biodome biodome);
    void AddSpeciesInBiome(int biomeId, string speciesName, int quantity);
    void AddEquipementInBiome(int biomeId, string equipementName);
    void RemoveSpeciesInBiome(int biomeId, string speciesName);
    void RemoveEquipementInBiome(int biomeId, string equipementName);
}