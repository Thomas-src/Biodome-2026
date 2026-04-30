using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using BioDomes.Infrastructures.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repository;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly BioDomesDbContext _context;
    private readonly ILogger<SpeciesRepository> _logger;

    public SpeciesRepository(BioDomesDbContext context, ILogger<SpeciesRepository> logger)
    {
        _context = context;
        _logger = logger;
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
        name =  name.ToLower();
        var entity = _context.Species
            .Include(s => s.Regim)
            .Include(s => s.TypeEntity)
            .Include(s => s.User)
            .FirstOrDefault(s =>s.Name.ToLower() == name);

        if (entity is null)
        {
            _logger.LogInformation("{} not found", name);
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
        var oldSpecies = _context.Species
            .Include(s => s.Regim)
            .Include(s => s.TypeEntity)
            .Include(s => s.User)
            .FirstOrDefault(s => s.Name == name);
        if (oldSpecies is null)
        {
            throw new ArgumentException("Espèce introuvable");
        }
        
        oldSpecies.Size = (int)species.Taille;
        oldSpecies.Weight = species.Poids;
        oldSpecies.Regim = FindRegime(species.DietType);
        oldSpecies.TypeEntity = FindType(species.SpeciesClassification);
        oldSpecies.Visible = species.IsVisible;
        oldSpecies.Avatar = species.Avatar;

        _context.Species.Update(oldSpecies);
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
    
    private SpeciesRegimeEntity FindRegime(DietType dietType)
    {
        return _context.SpeciesRegimes
                   .FirstOrDefault(r => r.Name == dietType.ToString())
               ?? throw new ArgumentException($"Régime '{dietType}' introuvable");
    }

    private SpeciesTypeEntity FindType(SpeciesClassification classification)
    {
        return _context.SpeciesTypes
                   .FirstOrDefault(t => t.Name == classification.ToString())
               ?? throw new ArgumentException($"Type '{classification}' introuvable");
    }
}