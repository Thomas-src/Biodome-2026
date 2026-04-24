namespace BioDomes.Infrastructures.entities;

public class SpeciesEntity
{
    public int IdSpecies { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
   
    public string TypeId { get; set; } = string.Empty;
    public SpeciesTypeEntity TypeEntity { get; set; }

    public string RegimId { get; set; } = string.Empty;
    public SpeciesRegimeEntity Regim { get; set; }
   
    public int Size { get; set; }
    public double Weight { get; set; } 
    public bool Visible { get; set; }

    // Lien avec User
    public string? UserId { get; set; }
    public UserEntity? User { get; set; }

    // Lien avec Biodomes
    public IList<BiodomeEntity>  Biodomes { get; set; } = new List<BiodomeEntity>();
}