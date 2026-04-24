namespace BioDomes.Infrastructures.entities;

public class SpeciesRegimeEntity
{
    public int IdRegime { get; set; }
    public string Name { get; set; }

    // Lien avec Species
    public IList<SpeciesEntity> Species { get; set; } = new List<SpeciesEntity>();
}