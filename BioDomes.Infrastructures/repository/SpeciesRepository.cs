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
            Enum.Parse<SpeciesClassification>(s.Type.Name),
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

        var entity = new entities.Species()
        {
            IdSpecies = incrementedId(),
            Avatar = created.Avatar,
            Name = created.Nom,
            Size = (int)created.Taille,
            Weight = created.Poids,
            UserId = userId,
            RegimId = _context.SpeciesRegim.FirstOrDefault(r => r.Name == created.DietType.ToString()).IdRegime,
            TypeId = _context.SpeciesTypes.FirstOrDefault(r => r.Name == created.SpeciesClassification.ToString()).IdType,
            Visible = created.IsVisible
        };

        _context.Species.Add(entity);
        _context.SaveChanges();
    }

    // Todo plus tard changer par une int pour avoir auto-increment
    private string incrementedId()
    {
        var maxId = _context.Species
            .AsNoTracking()
            .AsEnumerable() // 👈 important : bascule en mémoire
            .Select(s => int.TryParse(s.IdSpecies, out var id) ? id : 0)
            .DefaultIfEmpty(0)
            .Max();

        return (maxId + 1).ToString();
    }
}