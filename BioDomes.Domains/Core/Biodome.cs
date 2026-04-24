namespace BioDomes.Domains.Core;

public class Biodome
{
    private int _id;
    private string _nom;
    private double _temperature;
    private double _humidite;
    private string _image;
    private IReadOnlyList<Species> _species;
    public Biodome(int id, string nom, double temperature, double humidite, string image,IReadOnlyList<Species> species)
    {
        _id = id;
        _nom = nom;
        _temperature = temperature;
        _humidite = humidite;
        _image = image;
        _species = species;
    }
    public int Id
    {
        get { return _id; }
    }
    public IReadOnlyList<Species> Species { get { return _species; } }
    public string Nom { get { return _nom; } }
    public double Temperature { get { return _temperature; } }
    public double Humidite { get { return _humidite; } }
    public string Image{ get { return _image; } set { _image = value; } }

    public BiodomeEtat Etat => BiodomeEtatCompanion.DeterminerEtat(Temperature, Humidite);
}