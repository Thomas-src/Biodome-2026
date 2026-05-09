using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Repository.Exceptions;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.repository;

public class EquipementRepository(BioDomesDbContext dbContext) : IEquipementRepository
{
    public IReadOnlyList<Equipement> GetAllEquipements()
    {
        try
        {
            return dbContext.Equipment
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
        catch (Exception e)
        {
            throw new ReadEquipementException("Erreur lors de la lecture des équipement !", e);
        }
    }

    public void Save(Equipement created)
    {
        var u = dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == created.OwnerName.ToLower());

        if (u == null)
        {
            throw new ReadUserException("Utilisateur introuvable !");
        }

        var userId = u.Id;
        EquipmentEntity entity;

        if (created.Consumption == Consumptions.None)
        {
            entity = new EquipmentEntity
            {
                Name = created.Name,
                Avatar = created.Avatar,
                UserId = userId,
                Visible = created.Visibility,
                ProductionId = dbContext.Productions.FirstOrDefault(e => e.Name == created.Production.ToString())
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
                ProductionId = dbContext.Productions.FirstOrDefault(e => e.Name == created.Production.ToString())
                    .ProductionId,
                ConsumptionId = dbContext.Consumptions.FirstOrDefault(c => c.Name == created.Consumption.ToString())
                    .ConsumptionId
            };
        }

        try
        {
            dbContext.Add(entity);
            dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            throw new WriteEquipementException("Erreur lors de l'ajout de l'équipement en base de données", e);
        }
    }

    public void Delete(string name, string ownerName)
    {
        var entity = dbContext.Equipment.FirstOrDefault(e =>
            e.Name.ToLower() == name.ToLower() && e.User.UserName.ToLower() == ownerName.ToLower());

        if (entity is null)
        {
            return;
        }

        dbContext.Equipment.Remove(entity);
        dbContext.SaveChanges();
    }

    public void Update(string name, string ownerName, Equipement update)
    {
        var entity = dbContext.Equipment
            .Include(e => e.Consumption)
            .Include(e => e.Production)
            .Include(e => e.User)
            .FirstOrDefault(e =>
                e.Name.ToLower() == name.ToLower() && e.User.UserName.ToLower() == ownerName.ToLower());

        if (entity is null)
        {
            return;
        }

        entity.Name = update.Name;
        entity.Avatar = update.Avatar;
        entity.Visible = update.Visibility;

        // User
        var user = dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == update.OwnerName.ToLower());

        if (user != null)
        {
            entity.UserId = user.Id;
        }

        var production = dbContext.Productions.FirstOrDefault(p => p.Name == update.Production.ToString());

        if (production != null)
        {
            entity.ProductionId = production.ProductionId;
        }

        if (update.Consumption != Consumptions.None)
        {
            var consumption = dbContext.Consumptions.FirstOrDefault(c => c.Name == update.Consumption.ToString());

            if (consumption != null)
            {
                entity.ConsumptionId = consumption.ConsumptionId;
            }
        }
        else
        {
            entity.ConsumptionId = null;
        }

        dbContext.SaveChanges();
    }
}