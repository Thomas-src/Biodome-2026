using Microsoft.AspNetCore.Identity;
namespace BioDomes.Infrastructures.entities;

public class User : IdentityUser
{
    public string Avatar { get; set; } = string.Empty;

    // Lien avec Biodomes
    public IList<Biodomes> Biodomes { get; set; } = new List<Biodomes>();

    // Lien avec Equipement
    public IList<Equipment> Equipments { get; set; } = new List<Equipment>();

    // Lien avec ResearchOrganism
    public string ResearchOrganismeId { get; set; }
    public ResearchOrganism ResearchOrganism { get; set; }

    // Lien avec Species
    public IList<Species> Species { get; set; } = new List<Species>();
}