namespace BioDomes.Infrastructures.entities;

public class SpeciesTypeEntity
{
    public int IdType { get; set; }
    public string Name { get; set; }

    // Lien avec User
    public IList<SpeciesEntity> Species { get; set; } = new List<SpeciesEntity>();
}