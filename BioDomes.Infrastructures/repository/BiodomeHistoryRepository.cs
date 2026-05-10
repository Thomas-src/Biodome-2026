using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using BioDomes.Infrastructures.Repository;
using BioDomes.Infrastructures.repository.Mapper;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.repository;

public class BiodomeHistoryRepository(BioDomesDbContext context,ISpeciesService speciesService) : IBiodomeHistoryRepository
{
    public void Save(int biodomeId, int speciesId, int quantity)
    {
       var entry = new BiodomeHistoryEntity
        {
            BiodomeId = biodomeId,
            SpeciesId = speciesId,
            Delta = quantity,
            UpdateAt = DateTime.UtcNow,
        };

        context.BiodomeHistory.Add(entry);
        context.SaveChanges();
    }
    
    public void Remove(int biodomeId, int speciesId)
    {
        var currentQuantity = context.BiodomeHistory
            .Where(h => h.BiodomeId == biodomeId && h.SpeciesId == speciesId)
            .Sum(h => h.Delta);

        if (currentQuantity <= 0)
        {
            throw new Exception("Aucune quantité existante à supprimer");
        }

        var entry = new BiodomeHistoryEntity
        {
            BiodomeId = biodomeId,
            SpeciesId = speciesId,
            Delta = -currentQuantity,
            UpdateAt = DateTime.UtcNow,
        };

        context.BiodomeHistory.Add(entry);
        context.SaveChanges();
    }
    
    public IReadOnlyList<BiodomeSpeciesHistory> GetHistory(string biodomeSlug, string username)
    {
        var biodomeId = context.Biodomes
            .Where(b => b.Slug == biodomeSlug)
            .Select(b => b.idBiodomes)
            .FirstOrDefault();

        var history = context.BiodomeHistory
            .Include(h => h.Species) 
            .Where(h => h.BiodomeId == biodomeId)
            .OrderByDescending(h => h.UpdateAt).Include(biodomeHistoryEntity => biodomeHistoryEntity.Species)
            .ToList();

        return history.Select(h => new BiodomeSpeciesHistory
        {
            Species = SpeciesMapper.ToDomain(h.Species),
            Delta = h.Delta,
            UpdateAt = h.UpdateAt
        }).ToList();
    }
    public void DeleteHistory(int biodomeId)
    {
        var entries = context.BiodomeHistory
            .Where(h => h.BiodomeId == biodomeId)
            .ToList();

        foreach (var entry in entries)
        {
            context.BiodomeHistory.Remove(entry);
            context.SaveChanges();
        }
        
    }
}