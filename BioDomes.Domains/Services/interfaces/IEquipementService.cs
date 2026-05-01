using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Services.interfaces;

public interface IEquipementService
{
    IReadOnlyList<Equipement> GetAllEquipements();
    IReadOnlyList<Equipement> FilterOwn(IReadOnlyList<Equipement> all, string identityName);
    void Save(Equipement created, Stream openReadStream);
    public bool AlreadyExist(Equipement equipement);

    bool MakePublic(string equipementName, string ownerName);
    public Equipement? GetByKebabCase(string name, string ownerName);

    public void Delete(string name, string ownerName);
    public IReadOnlyList<Equipement> GetAvailableEquipements();
    public IReadOnlyList<Equipement> GetUnavailableEquipements();
    bool Update(Equipement oldEquipement, Equipement newEquipement, Stream? openReadStream);
}