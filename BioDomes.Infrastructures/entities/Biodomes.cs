namespace BioDomes.Infrastructures.entities;

public class Biodomes
{
    public string idBiodomes { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Avatar  { get; set; } = string.Empty;

    // Lien avec User
    public string userId { get; set; }
    public User user { get; set; }

    // Lien avec Species
    public IList<Species> Species { get; set; } = new List<Species>();

    // Lien avec Equipement
    public IList<Equipment> Equipment { get; set; } = new List<Equipment>();
}