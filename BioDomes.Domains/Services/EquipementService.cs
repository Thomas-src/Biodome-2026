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

    public void MakePublic(string equipementName)
    {
        var equipement = Get(equipementName);

        if (equipement is not null)
        {
            equipement.Visibility = true;

            _repository.Update(equipementName, equipement);
        }
    }

    public Equipement? Get(string name)
    {
        return _repository.GetAllEquipements().FirstOrDefault(e => e.Name == name);
    }

    public void Delete(string name)
    {
        var equipement = Get(name);

        if (equipement is null)
        {
            return;
        }
        
        if (!string.IsNullOrEmpty(equipement.Avatar))
        {
            _pictureRepository.Delete(equipement.Avatar, "equipements");
        }

        _repository.Delete(name);
    }

    public IReadOnlyList<Equipement> GetAvailableEquipements()
    {
        return _repository.GetAllEquipements()
            .Where(e => e.Visibility)
            .ToList();
    }

    public IReadOnlyList<Equipement> GetUnavailableEquipements()
    {
        return _repository.GetAllEquipements()
            .Where(e => !e.Visibility)
            .ToList();
    }

    public void Save(Equipement created, Stream openReadStream)
    {
        // 1. Sauvegarder l'image
        created.Avatar = _pictureRepository.Save(created.Avatar, openReadStream, "Equipements");
        
        // 2. Sauvegarder l'équipement
        _repository.Save(created);
    }
}