using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class EquipementService : IEquipementService
{
    private readonly IEquipementRepository _repository;

    public EquipementService(IEquipementRepository repository)
    {
        _repository = repository;
    }
    
    public IReadOnlyList<Equipement> GetAllEquipements()
    {
        return _repository.GetAllEquipements();
    }

    public IReadOnlyList<Equipement> FilterOwn(IReadOnlyList<Equipement> all, string identityName)
    {
        IList<Equipement> result = new List<Equipement>();

        foreach (Equipement equipement in all)
        {
            if (equipement.OwnerName.ToLower() == identityName.ToLower())
            {
                result.Add(equipement);
            }
        }
        
        return result.AsReadOnly();
    }
}