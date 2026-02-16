using BioDomes.Domains;

namespace BioDomes.Infrastructures;

public class SqlRepository : IRepository
{
    public SqlRepository()
    {
    }


    public IList<Espece> getallEspece()
    {
        IList<Espece> especes = new List<Espece>();

        especes.Add(new Espece("plante", "photosynthese", 12));
        especes.Add(new Espece("mammifere", "herbivore", 250));
        return especes;
    }
}