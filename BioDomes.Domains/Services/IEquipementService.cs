using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Services;

public interface IEquipementService
{
    IReadOnlyList<Equipement> GetAllEquipements();
    IReadOnlyList<Equipement> FilterOwn(IReadOnlyList<Equipement> all, string identityName);
    void Save(Equipement created, Stream openReadStream);
    public bool AlreadyExist(Equipement equipement);
    
    void MakePublic(string equipementName);
    public Equipement? Get(string name);
    public void Delete(string name);
    public IReadOnlyList<Equipement> GetAvailableEquipements();
    public IReadOnlyList<Equipement> GetUnavailableEquipements();
    bool Update(Equipement oldEquipement, Equipement newEquipement, Stream? openReadStream);
}