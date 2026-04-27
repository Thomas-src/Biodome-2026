using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.repository;

public class EquipementRepository : IEquipementRepository
{
    readonly BioDomesDbContext _dbContext;

    public EquipementRepository(BioDomesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IReadOnlyList<Equipement> GetAllEquipements()
    {
        return _dbContext.Equipment.Select(e => new Equipement(
            e.Name,
            Enum.Parse<Consumptions>(e.Consumption.Name),
            Enum.Parse<Production>(e.Production.Name),
            e.Avatar,
            e.Visible,
            e.User.UserName ?? "unknown"
            )).ToList();
    }
}