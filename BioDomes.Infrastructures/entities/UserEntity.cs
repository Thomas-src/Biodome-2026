using Microsoft.AspNetCore.Identity;
namespace BioDomes.Infrastructures.entities;

public class UserEntity : IdentityUser
{
    public string Avatar { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    
    public string? ResearchOrganismId { get; set; }
    public ResearchOrganismEntity? ResearchOrganism { get; set; }
    
    public IList<BiodomeEntity> Biodomes { get; set; } = new List<BiodomeEntity>();
    public IList<EquipmentEntity> Equipments { get; set; } = new List<EquipmentEntity>();
    public IList<SpeciesEntity> Species { get; set; } = new List<SpeciesEntity>();
}