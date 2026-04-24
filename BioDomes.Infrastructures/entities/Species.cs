namespace BioDomes.Infrastructures.entities;

public class Species
{
    public int IdSpecies { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
   
    public string TypeId { get; set; } = string.Empty;
    public SpeciesType Type { get; set; }

    public string RegimId { get; set; } = string.Empty;
    public SpeciesRegime Regim { get; set; }
   
    public int Size { get; set; }
    public double Weight { get; set; } 
    public bool Visible { get; set; }

    // Lien avec User
    public string? UserId { get; set; }
    public User? User { get; set; }

    // Lien avec Biodomes
    public IList<Biodomes>  Biodomes { get; set; } = new List<Biodomes>();
}