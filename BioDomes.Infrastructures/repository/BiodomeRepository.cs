using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Repository.Exceptions;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.repository;

public class BiodomeRepository : IBiodomeRepository
{
    private readonly BioDomesDbContext _context;

    public BiodomeRepository(BioDomesDbContext context)
    {
        _context = context;
    }

    /*
     * public IReadOnlyList<Biodome> GetBestThreeBiodome()
     {
         return _context.Biodomes
             .Select(b => new Biodome(b.idBiodomes, b.Name, b.Temperature, b.Humidity, b.Avatar))
             .OrderByDescending(b => b.Etat)
             .Take(3)
             .ToList<Biodome>();

     }
     */

    public IReadOnlyList<Biodome> GetAllBiodomes()
    {
        try
        {
            return _context.Biodomes
                .Select(b => new Biodome(
                    b.idBiodomes,
                    b.Name,
                    b.Temperature,
                    b.Humidity,
                    b.Avatar,
                    b.BiodomeSpecies.Select(bs => new Species(
                        bs.Species.Name,
                        bs.Species.Size,
                        bs.Species.Weight,
                        Enum.Parse<DietType>(bs.Species.Regim.Name),
                        Enum.Parse<SpeciesClassification>(bs.Species.TypeEntity.Name),
                        bs.Species.Avatar,
                        bs.Species.Visible,
                        bs.Species.User != null ? bs.Species.User.UserName : "unknown",
                        bs.Quantity
                    )).ToList(),
                    b.Equipment.Select(e => new Equipement(
                        e.Name,
                        e.Consumption != null ? Enum.Parse<Consumptions>(e.Consumption.Name) : Consumptions.None,
                        e.Production != null ? Enum.Parse<Production>(e.Production.Name) : Production.Lumière,
                        e.Avatar,
                        e.Visible,
                        e.User != null ? e.User.UserName : "unknown"
                    )).ToList(),
                    b.user.UserName
                ))
                .ToList();
        }
        catch (Exception e)
        {
            throw new ReadBiodomesException("Lecture impossible des biodomes en base de données !");
        }
    }

    public void Create(Biodome biodome)
    {
        var u = _context.Users.FirstOrDefault(u => u.UserName == biodome.CreatorName);
        if (u == null)
        {
            throw new ReadUserException("Plusieurs utilisateurs possedent le meme username");
        }

        string userId = u.Id;

        var entity = new BiodomeEntity
        {
            Name = biodome.Nom,
            Temperature = biodome.Temperature,
            Humidity = biodome.Humidite,
            Avatar = biodome.Image,
            userId = userId
        };

        try
        {
        _context.Biodomes.Add(entity);
        _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new WriteBiodomeException("Impossible d'ajouter le biodome en base de donnée !");
        }
        

    }

    public void Update(Biodome biodome)
    {
        var old = _context.Biodomes
            .FirstOrDefault(b => b.idBiodomes == biodome.Id);

        if (old == null)
        {
            throw new Exception("Biodome introuvable");
        }

        old.Name = biodome.Nom;
        old.Temperature = biodome.Temperature;
        old.Humidity = biodome.Humidite;
        old.Avatar = biodome.Image;

        _context.SaveChanges();
    }
    

  
    public void AddSpeciesInBiome(int biomeId, string speciesName, int quantity)
    {
        var specy = _context.Species.FirstOrDefault(s => s.Name == speciesName);

        if (specy == null)
            throw new Exception("Espèce introuvable");

        var existing = _context.BiodomeSpecies
            .FirstOrDefault(bs => bs.BiodomeId == biomeId && bs.SpeciesId == specy.IdSpecies);

        if (existing == null)
        {
                    var newEntry = new BiodomeSpeciesEntity
                    {
                        BiodomeId = biomeId,
                        SpeciesId = specy.IdSpecies,
                        Quantity = quantity
                    };
            
                    _context.BiodomeSpecies.Add(newEntry);
                    _context.SaveChanges();
        }
    }

    public void AddEquipementInBiome(int biomeId, string equipementName)
    {
        var biodome = _context.Biodomes.Include(b => b.Equipment).FirstOrDefault(b => b.idBiodomes == biomeId);
        var equipement = _context.Equipment.FirstOrDefault(e => e.Name == equipementName);

        if (!biodome.Equipment.Any(s => s.IdEquipment == equipement.IdEquipment))
        {
            biodome.Equipment.Add(equipement);
            _context.SaveChanges();
        }
    }

    public void RemoveSpeciesInBiome(int biomeId, string speciesName)
    {
        var specy = _context.Species.FirstOrDefault(s => s.Name == speciesName);

        if (specy == null)
            throw new Exception("Espèce introuvable");

        var existing = _context.BiodomeSpecies
            .FirstOrDefault(bs => bs.BiodomeId == biomeId && bs.SpeciesId == specy.IdSpecies);

        if (existing != null)
        {
            _context.BiodomeSpecies.Remove(existing);
            _context.SaveChanges();
        }
    }

    public void RemoveEquipementInBiome(int biomeId, string equipementName)
    {
        var biodome = _context.Biodomes.Include(b => b.Equipment).FirstOrDefault(b => b.idBiodomes == biomeId);
        var equipement = _context.Equipment.FirstOrDefault(e => e.Name == equipementName);

        if (biodome.Equipment.Any(s => s.IdEquipment == equipement.IdEquipment))
        {
            biodome.Equipment.Remove(equipement);
            _context.SaveChanges();
        }
    }
}