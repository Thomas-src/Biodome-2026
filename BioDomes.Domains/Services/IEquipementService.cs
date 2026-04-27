using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Services;

public interface IEquipementService
{
    IReadOnlyList<Equipement> GetAllEquipements();
    IReadOnlyList<Equipement> FilterOwn(IReadOnlyList<Equipement> all, string identityName);
    void Save(Equipement created, Stream openReadStream);
    public bool AlreadyExist(Equipement equipement);
}