using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class EquipementService : IEquipementService
{
    private readonly IEquipementRepository _repository;
    private readonly IPictureRepository _pictureRepository;

    public EquipementService(IEquipementRepository repository,  IPictureRepository pictureRepository)
    {
        _repository = repository;
        _pictureRepository = pictureRepository;
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

    public bool AlreadyExist(Equipement equipement)
    {
        foreach (var eq in _repository.GetAllEquipements())
        {
            if (eq.Equals(equipement))
            {
                return true;
            }
        }
        
        return false;
    }
    
    public void Save(Equipement created, Stream openReadStream)
    {
        // 1. Sauvegarder l'image
        created.Avatar = _pictureRepository.Save(created.Avatar, openReadStream, "Equipements");
        
        // 2. Sauvegarder l'équipement
        _repository.Save(created);
    }
}