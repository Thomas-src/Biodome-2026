using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.repository;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repository;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly BioDomesDbContext _context;

    public SpeciesRepository(BioDomesDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Species> GetAllSpecies()
    {
        return _context.Species.Select(s => new Species(s.Name,
            s.Size,
            s.Weight,
            Enum.Parse<DietType>(s.Regim.Name),
            Enum.Parse<SpeciesClassification>(s.TypeEntity.Name),
            s.Avatar,
            s.Visible,
            s.User.UserName)
        ).ToList();
    }

    public void Save(Species created)
    {
        // 1. Récuperer l'id de l'utilisateur
        var u = _context.Users.FirstOrDefault(u => u.UserName == created.OwnerName);
        if (u == null)
        {
            throw new ArgumentException("Plusieurs utilisateurs possedent le meme username");
        }

        string userId = u.Id;

        // 2. Enregistrer l'espece

        var entity = new entities.SpeciesEntity()
        {
            Avatar = created.Avatar,
            Name = created.Nom,
            Size = (int)created.Taille,
            Weight = created.Poids,
            UserId = userId,
            RegimId = _context.SpeciesRegimes.FirstOrDefault(r => r.Name == created.DietType.ToString()).IdRegime,
            TypeId = _context.SpeciesTypes.FirstOrDefault(r => r.Name == created.SpeciesClassification.ToString())
                .IdType,
            Visible = created.IsVisible
        };

        _context.Species.Add(entity);
        _context.SaveChanges();
    }

    public Species? GetSpecies(string name)
    {
        var entity = _context.Species
            .Include(s => s.Regim)
            .Include(s => s.TypeEntity)
            .Include(s => s.User)
            .FirstOrDefault(s => s.Name == name);

        if (entity is null)
        {
            return null;
        }

        return new Species(
            entity.Name,
            entity.Size,
            entity.Weight,
            Enum.Parse<DietType>(entity.Regim.Name),
            Enum.Parse<SpeciesClassification>(entity.TypeEntity.Name),
            entity.Avatar,
            entity.Visible,
            entity.User.UserName
        );
    }

    public void Update(string name, Species species)
    {
        var entity = _context.Species.FirstOrDefault(s => s.Name == name);
        if (entity is null)
        {
            throw new ArgumentException("Espèce introuvable");
        }

        entity.Name = species.Nom;
        entity.Size = (int)species.Taille;
        entity.Weight = species.Poids;
        entity.Avatar = species.Avatar;
        entity.Visible = species.IsVisible;
        entity.RegimId = _context.SpeciesRegimes.First(r => r.Name == species.DietType.ToString()).IdRegime;
        entity.TypeId = _context.SpeciesTypes.First(t => t.Name == species.SpeciesClassification.ToString()).IdType;
        var user = _context.Users.FirstOrDefault(u => u.UserName == species.OwnerName);
        if (user is null)
        {
            throw new ArgumentException("Utilisateur introuvable");
        }

        entity.UserId = user.Id;
        _context.SaveChanges();
    }

    public void Delete(string name)
    {
        var entity = _context.Species
            .FirstOrDefault(s => s.Name == name);

        if (entity is null)
        {
            return;
        }

        _context.Species.Remove(entity);
        _context.SaveChanges();
    }
}