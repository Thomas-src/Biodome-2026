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
        biodomes.Add(new Biodome("123546546","Terre", 33,33.4,GetallEspece(), "1.webp"));
        biodomes.Add(new Biodome("123546548","Lune", 33,25.4,GetallEspece(), "1.webp"));
        biodomes.Add(new Biodome("123546547","Mars", 300,100,GetallEspece(), "1.webp"));

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

    public IList<Biodome> GetAllDomes()
    {
        IList<Biodome> biodomes = new List<Biodome>();
        biodomes.Add(new Biodome("1","Terre", 33,33.4,GetallEspece()));
        biodomes.Add(new Biodome("2","Lune", 33,25.4,GetallEspece()));
        biodomes.Add(new Biodome("3","Lune", 33,25.4,GetallEspece()));
        biodomes.Add(new Biodome("4","Jupiter", 198,5,GetallEspece()));

        return biodomes;
    }
}