namespace BioDomes.Infrastructures.entities;

public class SpeciesType
{
    public string IdType { get; set; }
    public string Name { get; set; }

    // Lien avec User
    public IList<Species> Species { get; set; } = new List<Species>();
}