using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Extensions;
using BioDomes.Infrastructures.entities;

namespace BioDomes.Infrastructures.repository.Mapper;

public static class BiodomeMapper
{
    public static Biodome BiodomeToDomain(BiodomeEntity entity)
    {
        return new Biodome(
            entity.Name,
            entity.Temperature,
            entity.Humidity,
            entity.Avatar,
            entity.BiodomeSpecies.Select(bs => new Species(
                bs.Species.Name,
                bs.Species.Size,
                bs.Species.Weight,
                Enum.Parse<DietType>(bs.Species.Regim.Name),
                Enum.Parse<SpeciesClassification>(bs.Species.TypeEntity.Name),
                bs.Species.Avatar,
                bs.Species.Visible,
                bs.Species.User.UserName,
                bs.Quantity
            )).ToList(),
            entity.Equipment.Select(e => new Equipement(
                e.Name,
                e.Consumption != null ? Enum.Parse<Consumptions>(e.Consumption.Name) : Consumptions.None,
                Enum.Parse<Production>(e.Production.Name),
                e.Avatar,
                e.Visible,
                e.User.UserName
            )).ToList(),
            entity.user.UserName,
            entity.Name.ToSlug()
        );
    }

    public static BiodomeEntity BiodomeToEntity(Biodome biodome, string userId)
    {
        return new BiodomeEntity
        {
            Name = biodome.Nom,
            Temperature = biodome.Temperature,
            Humidity = biodome.Humidite,
            Avatar = biodome.Image,
            userId = userId,
            Slug = biodome.Slug,
        };
    }

    public static void UpdateEntity(Biodome biodome, BiodomeEntity entity)
    {
        entity.Name = biodome.Nom;
        entity.Temperature = biodome.Temperature;
        entity.Humidity = biodome.Humidite;
        entity.Avatar = biodome.Image;
        entity.Slug = biodome.Slug;
    }
}