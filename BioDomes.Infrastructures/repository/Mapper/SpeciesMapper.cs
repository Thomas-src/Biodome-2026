using BioDomes.Domains.Core;
using BioDomes.Infrastructures.entities;

namespace BioDomes.Infrastructures.repository.Mapper;

public static class SpeciesMapper
{
    public static Species ToDomain(SpeciesEntity entity)
    {
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

    public static SpeciesEntity ToEntity(Species species, string userId, int regimId, int typeId)
    {
        return new SpeciesEntity
        {
            Name = species.Nom,
            Size = (int)species.Taille,
            Weight = species.Poids,
            Avatar = species.Avatar,
            Visible = species.IsVisible,
            UserId = userId,
            RegimId = regimId,
            TypeId = typeId
        };
    }
}