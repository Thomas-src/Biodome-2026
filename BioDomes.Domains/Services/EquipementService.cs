using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class EquipementService(IEquipementRepository repository, IPictureRepository pictureRepository)
    : IEquipementService
{
    private static string EQUIPEMENT = "equipements";

    public IReadOnlyList<Equipement> GetAllEquipements() => repository.GetAllEquipements();

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

    public bool AlreadyExist(Equipement equipement) => repository.GetAllEquipements().Any(e =>
        e.Name.Equals(equipement.Name, StringComparison.OrdinalIgnoreCase) &&
        (e.Visibility || e.OwnerName == equipement.OwnerName));

    public bool MakePublic(string equipementName, string ownerName)
    {
        var equipement = GetByKebabCase(equipementName, ownerName);

        if (equipement is null)
        {
            return false;
        }
        
        var alreadyVisible = repository.GetAllEquipements().Any(s =>
            s.Name.Equals(equipementName, StringComparison.CurrentCultureIgnoreCase) &&
            s.Visibility &&
            !s.OwnerName.Equals(ownerName, StringComparison.CurrentCultureIgnoreCase));

        if (alreadyVisible)
        {
            return false;
        }

        equipement.Visibility = true;

        repository.Update(equipementName, ownerName, equipement);
        return true;
    }

    public Equipement? GetByKebabCase(string name, string ownerName) => repository.GetAllEquipements()
        .FirstOrDefault(e => e.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                             && e.OwnerName.Equals(ownerName, StringComparison.CurrentCultureIgnoreCase));

    public void Delete(string name, string ownerName)
    {
        var equipement = GetByKebabCase(name, ownerName);

        if (equipement is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(equipement.Avatar))
        {
            pictureRepository.Delete(equipement.Avatar, EQUIPEMENT);
        }

        repository.Delete(name, ownerName);
    }

    public IReadOnlyList<Equipement> GetAvailableEquipements() =>
        repository.GetAllEquipements()
            .Where(e => e.Visibility)
            .ToList();


    public IReadOnlyList<Equipement> GetUnavailableEquipements() =>
        repository.GetAllEquipements()
            .Where(e => !e.Visibility)
            .ToList();


    public void Save(Equipement created, Stream openReadStream)
    {
        // 1. Sauvegarder l'image
        created.Avatar = pictureRepository.Save(created.Avatar, openReadStream, EQUIPEMENT);

        // 2. Sauvegarder l'équipement
        repository.Save(created);
    }

    public bool Update(Equipement oldEquipement, Equipement newEquipement, Stream? openReadStream)
    {
        try
        {
            if (oldEquipement == null || newEquipement == null)
            {
                return false;
            }

            oldEquipement.Visibility = newEquipement.Visibility;
            if (openReadStream != null)
            {
                if (!string.IsNullOrEmpty(oldEquipement.Avatar))
                {
                    pictureRepository.Delete(oldEquipement.Avatar, EQUIPEMENT);
                }

                var filename = pictureRepository.Save(
                    newEquipement.Avatar,
                    openReadStream,
                    EQUIPEMENT
                );
                newEquipement.Avatar = filename;
            }

            repository.Update(oldEquipement.Name, oldEquipement.OwnerName, newEquipement);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("============== UPDATE EXCEPTION ==============");
            Console.WriteLine(ex);
            return false;
        }
    }
}