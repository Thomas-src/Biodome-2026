using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiodomeRepository
{
    IReadOnlyList<Biodome> GetAllBiodomes();
    
    void Create(Biodome biodome);
    void Update(Biodome biodome);
    void Delete(string id);
    Biodome GetBiodomeById(int biodomeId);
    void AddSpecyInBiome(int biomeId, string speciesName);
    void AddEquipementInBiome(int biomeId, string equipementName);
}