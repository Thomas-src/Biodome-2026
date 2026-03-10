using BioDomes.Domains;

namespace BioDomes.Infrastructures;

public class SqlRepository : IRepository
{
    public SqlRepository()
    {
    }


    public IList<Espece> GetallEspece()
    {
        IList<Espece> especes = new List<Espece>();

        especes.Add(new Espece("Lion","Panthera Leo", "Carnivore", 200));
        especes.Add(new Espece("Zèbre","Equus Quagga", "Herbivore", 130));
        return especes;
    }
}