namespace BioDomes.Infrastructures.entities;

public class SpeciesRegime
{
    public string IdRegime { get; set; }
    public string Name { get; set; }

    // Lien avec Species
    public IList<Species> Species { get; set; } = new List<Species>();
}