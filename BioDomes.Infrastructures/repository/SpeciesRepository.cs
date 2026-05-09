using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Repository.Exceptions;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using BioDomes.Infrastructures.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repository;

public class SpeciesRepository(BioDomesDbContext context) : ISpeciesRepository
{
    public Species? FindSpecies(string speciesName, string username)
    {
        return GetAllSpecies().FirstOrDefault(s=>s.Nom.ToLower().Equals(speciesName.ToLower()) && s.OwnerName.ToLower().Equals(username.ToLower()));
    }
    public IReadOnlyList<Species> GetAllSpecies()
    {
        try
        {
            return context.Species.Select(s => new Species(s.Name,
                s.Size,
                s.Weight,
                Enum.Parse<DietType>(s.Regim.Name),
                Enum.Parse<SpeciesClassification>(s.TypeEntity.Name),
                s.Avatar,
                s.Visible,
                s.User.UserName)
            ).ToList();
        }
        catch (Exception e)
        {
            throw new ReadSpeciesException("Erreur lors de la lecture des espèces !", e);
        }
    }

    public void Save(Species created)
    {
        // 1. Récuperer l'id de l'utilisateur
        var u = context.Users.FirstOrDefault(u => u.UserName == created.OwnerName);
        if (u is null)
        {
            throw new ReadUserException("Utilisateur introuvable");
        }

        string userId = u.Id;

        // 2. Enregistrer l'espece

        var entity = new SpeciesEntity()
        {
            Avatar = created.Avatar,
            Name = created.Nom,
            Size = (int)created.Taille,
            Weight = created.Poids,
            UserId = userId,
            RegimId = context.SpeciesRegimes.FirstOrDefault(r => r.Name == created.DietType.ToString()).IdRegime,
            TypeId = context.SpeciesTypes.FirstOrDefault(r => r.Name == created.SpeciesClassification.ToString())
                .IdType,
            Visible = created.IsVisible
        };

        try
        {
            context.Species.Add(entity);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new WriteSpeciesException($"Erreur lors de l'ajout de l'espece {entity.Name} en base de données !",
                e);
        }
    }


    public void Update(string name, string ownerName, Species species)
    {
        var oldSpecies = context.Species
            .Include(s => s.Regim)
            .Include(s => s.TypeEntity)
            .Include(s => s.User)
            .FirstOrDefault(s =>
                s.Name.ToLower() == name.ToLower() && s.User.UserName.ToLower() == ownerName.ToLower());
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

        context.Species.Update(oldSpecies);
        context.SaveChanges();
    }

    public void Delete(string name)
    {
        var entity = context.Species
            .FirstOrDefault(s => s.Name == name);

        if (entity is null)
        {
            return;
        }

        context.Species.Remove(entity);
        context.SaveChanges();
    }

    private SpeciesRegimeEntity FindRegime(DietType dietType)
    {
        return context.SpeciesRegimes
                   .FirstOrDefault(r => r.Name == dietType.ToString())
               ?? throw new ArgumentException($"Régime '{dietType}' introuvable");
    }

    private SpeciesTypeEntity FindType(SpeciesClassification classification)
    {
        return context.SpeciesTypes
                   .FirstOrDefault(t => t.Name == classification.ToString())
               ?? throw new ArgumentException($"Type '{classification}' introuvable");
    }
}