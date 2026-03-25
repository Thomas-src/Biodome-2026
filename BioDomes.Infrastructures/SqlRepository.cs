using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;

namespace BioDomes.Infrastructures;

public class SqlRepository : IRepository
{
    public SqlRepository()
    {
    }

    public IList<Biodome> GetBestThreeBiodome()
    {
        IList<Biodome> biodomes = new List<Biodome>();
        biodomes.Add(new Biodome("Terre", 33,33.4,GetallEspece()));
        biodomes.Add(new Biodome("Lune", 33,25.4,GetallEspece()));
        biodomes.Add(new Biodome("Mars", 300,100,GetallEspece()));

        return biodomes;
    }

    public IList<Espece> GetallEspece()
    {
        IList<Espece> especes = new List<Espece>();

        especes.Add(new Espece("Lion","Panthera Leo", "Carnivore", 200));
        especes.Add(new Espece("Zèbre","Equus Quagga", "Herbivore", 130));
        especes.Add(new Espece("Mammiferes","Chat", "Poisson", 90));
        return especes;
    }

    public void Save(Espece espece)
    {
        //TODO
    }
}