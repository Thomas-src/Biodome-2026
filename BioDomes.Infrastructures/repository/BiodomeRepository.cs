using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
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
        return _context.Biodomes
            .Select(b => new Biodome(
                b.idBiodomes,
                b.Name,
                b.Temperature,
                b.Humidity,
                b.Avatar,
                b.Species.Select(s => new Species(
                    s.Name,
                    s.Size,
                    s.Weight,
                    Enum.Parse<DietType>(s.Regim.Name),
                    Enum.Parse<SpeciesClassification>(s.TypeEntity.Name),
                    s.Avatar,
                    s.Visible,
                    s.User != null ? s.User.UserName : "unknown"
                )).ToList(),
                b.Equipment.Select(e => new Equipement(
                    e.Name,
                    Enum.Parse<Consumptions>(e.Consumption.Name),
                    Enum.Parse<Production>(e.Production.Name),
                    e.Avatar,
                    e.Visible,
                    e.User != null ? e.User.UserName : "unknown"
                )).ToList(),
                b.user.UserName
            ))
            .ToList();
    }

    public void Create(Biodome biodome)
    {
        var u = _context.Users.FirstOrDefault(u => u.UserName == biodome.CreatorName);
        if (u == null)
        {
            throw new ArgumentException("Plusieurs utilisateurs possedent le meme username");
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
        _context.Biodomes.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Biodome biodome)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Biodome GetBiodomeById(int biodomeId)
    {
        return _context.Biodomes
            .Where(b => b.idBiodomes == biodomeId)
            .Select(b => new Biodome(
                b.idBiodomes,
                b.Name,
                b.Temperature,
                b.Humidity,
                b.Avatar,
                b.Species.Select(s => new Species(
                    s.Name,
                    s.Size,
                    s.Weight,
                    Enum.Parse<DietType>(s.Regim.Name),
                    Enum.Parse<SpeciesClassification>(s.TypeEntity.Name),
                    s.Avatar,
                    s.Visible,
                    s.User != null ? s.User.UserName : "unknown"
                )).ToList(),
                b.Equipment.Select(e => new Equipement(
                    e.Name,
                    Enum.Parse<Consumptions>(e.Consumption.Name),
                    Enum.Parse<Production>(e.Production.Name),
                    e.Avatar,
                    e.Visible,
                    e.User != null ? e.User.UserName : "unknown"
                )).ToList(),
                b.user.UserName
            ))        
            .AsNoTracking()
            .FirstOrDefault();
    }
}