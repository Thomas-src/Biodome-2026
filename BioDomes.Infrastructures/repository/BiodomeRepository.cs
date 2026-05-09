using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Repository.Exceptions;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using BioDomes.Infrastructures.repository.Mapper;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.repository;

public class BiodomeRepository(BioDomesDbContext context, IBiodomeHistoryRepository biodomeHistoryRepository) : IBiodomeRepository
{
    public IReadOnlyList<Biodome> GetAllBiodomes()
    {
        try
        {
            return context.Biodomes
                .Include(b => b.BiodomeSpecies)
                .ThenInclude(bs => bs.Species)
                .ThenInclude(s => s.Regim)
                .Include(b => b.BiodomeSpecies)
                .ThenInclude(bs => bs.Species)
                .ThenInclude(s => s.TypeEntity)
                .Include(b => b.BiodomeSpecies)
                .ThenInclude(bs => bs.Species)
                .ThenInclude(s => s.User)
                .Include(b => b.Equipment)
                .ThenInclude(e => e.User)
                .Include(b => b.Equipment)
                .ThenInclude(e => e.Consumption)
                .Include(b => b.Equipment)
                .ThenInclude(e => e.Production)
                .Include(b => b.user)
                .ToList()
                .Select(BiodomeMapper.BiodomeToDomain)
                .ToList();
        }
        catch (Exception e)
        {
            throw new ReadBiodomesException("Lecture impossible des biodomes en base de données !", e);
        }
    }

    public void Create(Biodome biodome)
    {
        var userId = GetUserIdByUsername(biodome.CreatorName);
        var entity = BiodomeMapper.BiodomeToEntity(biodome, userId);

        try
        {
            context.Biodomes.Add(entity);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new WriteBiodomeException("Impossible d'ajouter le biodome en base de donnée !",e);
        }
    }

    public void Update(Biodome biodome)
    {
        var old = context.Biodomes.FirstOrDefault(b => b.Slug.Equals(biodome.Slug));

        if (old == null)
        {
            throw new Exception("Biodome introuvable");
        }

        BiodomeMapper.UpdateEntity(biodome, old);
        context.SaveChanges();
    }


    public void AddSpeciesInBiome(string biomeSlug, string speciesName, int quantity)
    {
        var specy = context.Species.FirstOrDefault(s => s.Name == speciesName);

        if (specy == null)
        {
            throw new Exception("Espèce introuvable");
        }

        var biodome = GetBiodomeEntityBySlug(biomeSlug);
       
        biodomeHistoryRepository.Save(biodome.idBiodomes, specy.IdSpecies, quantity);
       
        var existing = context.BiodomeSpecies
            .FirstOrDefault(bs => bs.BiodomeId == biodome.idBiodomes && bs.SpeciesId == specy.IdSpecies);

        if (existing == null)
        {
            var newEntry = new BiodomeSpeciesEntity
            {
                BiodomeId = biodome.idBiodomes,
                SpeciesId = specy.IdSpecies,
                Quantity = quantity
            };

            context.BiodomeSpecies.Add(newEntry);
        }
        else
        {
            existing.Quantity += quantity;
        }

        context.SaveChanges();
    }

    public void AddEquipementInBiome(string biomeSlug, string equipementName)
    {
        var biodome = context.Biodomes.Include(b => b.Equipment).FirstOrDefault(b => b.Slug == biomeSlug);
        var equipement = context.Equipment.FirstOrDefault(e => e.Name == equipementName);

        if (biodome != null && equipement != null)
        {
            if (!biodome.Equipment.Any(s => s.IdEquipment == equipement.IdEquipment))
            {
                biodome.Equipment.Add(equipement);
                context.SaveChanges();
            }
        }
    }

    public void RemoveSpeciesInBiome(string biomeSlug, string speciesName)
    {
        var specy = context.Species.FirstOrDefault(s => s.Name == speciesName);
        var biodome = context.Biodomes.FirstOrDefault(bs => bs.Slug == biomeSlug);
        if (biodome != null)
        {
            biodomeHistoryRepository.Remove(biodome.idBiodomes, specy.IdSpecies);
            var existing = context.BiodomeSpecies
                .FirstOrDefault(bs => bs.BiodomeId == biodome.idBiodomes && bs.SpeciesId == specy.IdSpecies);
            if (existing != null)
            {
                context.BiodomeSpecies.Remove(existing);
                context.SaveChanges();
            }
        }
    }

    public void RemoveEquipementInBiome(string biomeSlug, string equipementName)
    {
        var biodome = context.Biodomes.Include(b => b.Equipment).FirstOrDefault(b => b.Slug == biomeSlug);
        var equipement = context.Equipment.FirstOrDefault(e => e.Name == equipementName);

        if (biodome.Equipment.Any(s => s.IdEquipment == equipement.IdEquipment))
        {
            biodome.Equipment.Remove(equipement);
            context.SaveChanges();
        }
    }

    public Biodome? FindBiodome(string biodomeSlug)
    {
        var biodome = GetBiodomeEntityBySlug(biodomeSlug);
        return BiodomeMapper.BiodomeToDomain(biodome);
    }

    public void DeleteBiodome(string biomeSlug)
    {
        var biodome = GetBiodomeEntityBySlug(biomeSlug);

        try
        {
            context.Biodomes.Remove(biodome);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new DeleteBiodomeException("Impossible de supprimer le biodome !", e);
        }
    }

    private BiodomeEntity GetBiodomeEntityBySlug(string slug)
    {
        var biodome = GetBiodomeWithRelations(slug);

        if (biodome == null)
        {
            throw new Exception("Biodome introuvable");
        }

        return biodome;
    }

    private string GetUserIdByUsername(string username)
    {
        var user = context.Users
            .FirstOrDefault(u => u.UserName == username);

        if (user == null)
        {
            throw new ReadUserException("Aucun utilisateur trouvé");
        }

        return user.Id;
    }

    private BiodomeEntity? GetBiodomeWithRelations(string slug)
    {
        return context.Biodomes
            .Include(b => b.BiodomeSpecies)
            .ThenInclude(bs => bs.Species)
            .ThenInclude(s => s.Regim)
            .Include(b => b.BiodomeSpecies)
            .ThenInclude(bs => bs.Species)
            .ThenInclude(s => s.TypeEntity)
            .Include(b => b.BiodomeSpecies)
            .ThenInclude(bs => bs.Species)
            .ThenInclude(s => s.User)
            .Include(b => b.Equipment)
            .ThenInclude(e => e.User)
            .Include(b => b.Equipment)
            .ThenInclude(e => e.Consumption)
            .Include(b => b.Equipment)
            .ThenInclude(e => e.Production)
            .Include(b => b.user)
            .FirstOrDefault(b => b.Slug == slug);
    }
}