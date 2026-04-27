using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Repository;

public interface IEquipementRepository
{
    public IReadOnlyList<Equipement> GetAllEquipements();
    void Save(Equipement created);
}