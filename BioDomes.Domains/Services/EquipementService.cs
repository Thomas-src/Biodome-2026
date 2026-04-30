using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class EquipementService(IEquipementRepository repository, IPictureRepository pictureRepository) : IEquipementService
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

    public bool AlreadyExist(Equipement equipement)
    => repository.GetAllEquipements().Contains(equipement);
    

    public void MakePublic(string equipementName)
    {
        var equipement = Get(equipementName);

        if (equipement is not null)
        {
            equipement.Visibility = true;

            repository.Update(equipementName, equipement);
        }
    }

    public Equipement? Get(string name) => repository.GetAllEquipements().FirstOrDefault(e => e.Name.ToLower() == name.ToLower());

    public void Delete(string name)
    {
        var equipement = Get(name);

        if (equipement is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(equipement.Avatar))
        {
            pictureRepository.Delete(equipement.Avatar, EQUIPEMENT);
        }

        repository.Delete(name);
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

            repository.Update(oldEquipement.Name, newEquipement);

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