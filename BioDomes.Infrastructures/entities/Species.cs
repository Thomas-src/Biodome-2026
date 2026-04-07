namespace BioDomes.Infrastructures.entities;

public class Species
{
    public String IdSpecies { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
   
    public string Type { get; set; } = string.Empty;
    public string Regim { get; set; } = string.Empty;
   
    public double Size { get; set; }
    public double Weight { get; set; } 
    public bool Visible { get; set; }
}