namespace BioDomes.Infrastructures.entities;

public class ResearchOrganism
{
    public string IdOrgansim { get; set; }
    public string Name { get; set; }

    // Lien avec User
    public IList<User> Users { get; set; } = new List<User>();
}