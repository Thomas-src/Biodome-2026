namespace BioDomes.Infrastructures.entities;

public class SpeciesRegimeEntity
{
    public string IdRegime { get; set; }
    public string Name { get; set; }

    // Lien avec Species
    public IList<SpeciesEntity> Species { get; set; } = new List<SpeciesEntity>();
}