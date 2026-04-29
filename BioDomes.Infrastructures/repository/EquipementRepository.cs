using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
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
        return _dbContext.Equipment
            .Include(e => e.Consumption)
            .Include(e => e.Production)
            .Include(e => e.User)
            .Select(e => new Equipement(
                e.Name,
                e.Consumption != null 
                    ? Enum.Parse<Consumptions>(e.Consumption.Name)
                    : Consumptions.None,
                Enum.Parse<Production>(e.Production.Name),
                e.Avatar,
                e.Visible,
                e.User.UserName ?? "unknown"
            ))
            .ToList();
    }

    public void Save(Equipement created)
    {
        var u = _dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == created.OwnerName.ToLower());
        
        if (u == null)
        {
            throw new ArgumentException("Plusieurs utilisateurs possedent le meme username");
        }

        string userId = u.Id;
        EquipmentEntity entity;

        if (created.Consumption == Consumptions.None)
        {
            entity = new EquipmentEntity
            {
                Name = created.Name,
                Avatar = created.Avatar,
                UserId = userId,
                Visible = created.Visibility,
                ProductionId = _dbContext.productions.FirstOrDefault(e => e.Name == created.Production.ToString())
                    .ProductionId
            };
        }
        else
        {
            entity = new EquipmentEntity
            {
                Name = created.Name,
                Avatar = created.Avatar,
                UserId = userId,
                Visible = created.Visibility,
                ProductionId = _dbContext.productions.FirstOrDefault(e => e.Name == created.Production.ToString())
                    .ProductionId,
                ConsumptionId = _dbContext.consumptions.FirstOrDefault(c => c.Name == created.Consumption.ToString())
                    .ConsumptionId
            };
        }

        _dbContext.Add(entity);
        _dbContext.SaveChanges();


    }

    public void Delete(string name)
    {
        var entity = _dbContext.Equipment.FirstOrDefault(e => e.Name == name);

        if (entity is null)
        {
            return;
        }
        
        _dbContext.Equipment.Remove(entity);
        _dbContext.SaveChanges();
    }

    public void Update(string name, Equipement update)
    {
        var entity = _dbContext.Equipment
            .Include(e => e.Consumption)
            .Include(e => e.Production)
            .Include(e => e.User)
            .FirstOrDefault(e => e.Name == name);

        if (entity is null)
        {
            return;
        } 

        entity.Name = update.Name;
        entity.Avatar = update.Avatar;
        entity.Visible = update.Visibility;

        // User
        var user = _dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == update.OwnerName.ToLower());

        if (user != null)
        {
            entity.UserId = user.Id;
        }

        var production = _dbContext.productions.FirstOrDefault(p => p.Name == update.Production.ToString());

        if (production != null)
        {
            entity.ProductionId = production.ProductionId;
        }

        if (update.Consumption != Consumptions.None)
        {
            var consumption = _dbContext.consumptions.FirstOrDefault(c => c.Name == update.Consumption.ToString());

            if (consumption != null)
            {
                entity.ConsumptionId = consumption.ConsumptionId;
            }
        }
        else
        {
            entity.ConsumptionId = null;
        }

        _dbContext.SaveChanges();
    }
}