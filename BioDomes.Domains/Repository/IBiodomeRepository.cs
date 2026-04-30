using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiodomeRepository
{
    IReadOnlyList<Biodome> GetAllBiodomes();
    
    void Create(Biodome biodome);
    void Update(Biodome biodome);
    void Delete(string id);
    Biodome GetBiodomeById(int biodomeId);
    void AddSpecyInBiome(int biomeId, string speciesName, int quantity);
    void AddEquipementInBiome(int biomeId, string equipementName);
    void RemoveSpecyInBiome(int biomeId, string speciesName);
    void RemoveEquipementInBiome(int biomeId, string equipementName);
}