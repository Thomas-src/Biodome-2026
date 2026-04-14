using Microsoft.AspNetCore.Identity;
namespace BioDomes.Infrastructures.entities;

public class User : IdentityUser
{
    public string Avatar { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    
    public string ResearchOrganismeId { get; set; } = string.Empty;
    public ResearchOrganism ResearchOrganism { get; set; } = null!;
    
    public IList<Biodomes> Biodomes { get; set; } = new List<Biodomes>();
    public IList<Equipment> Equipments { get; set; } = new List<Equipment>();
    public IList<Species> Species { get; set; } = new List<Species>();
}