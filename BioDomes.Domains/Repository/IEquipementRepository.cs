using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Repository;

public interface IEquipementRepository
{
    public IReadOnlyList<Equipement> GetAllEquipements();
    void Save(Equipement created);
    void Delete(string name, string ownerName);
    void Update(string name, string ownerName, Equipement update);
}