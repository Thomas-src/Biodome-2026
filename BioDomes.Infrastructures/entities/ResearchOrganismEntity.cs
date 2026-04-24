namespace BioDomes.Infrastructures.entities;

public class ResearchOrganismEntity
{
    public string ResearchOrganismId { get; set; }
    public string Name { get; set; }

    // Lien avec User
    public IList<UserEntity>? Users { get; set; } = new List<UserEntity>();
}