using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Core;

public class Biodome
{
    private int _id;
    private string _nom;
    private double _temperature;
    private double _humidite;
    private string _image;
    private IReadOnlyList<Species> _species;
    private IReadOnlyList<Equipement> _equipements;
    private string _creatorName; 
    public Biodome(int id, string nom, double temperature, double humidite, string image,IReadOnlyList<Species> species, IReadOnlyList<Equipement> equipements, string creatorName)
    {
        _id = id;
        _nom = nom;
        _temperature = temperature;
        _humidite = humidite;
        _image = image;
        _species = species;
        _equipements = equipements;
        _creatorName = creatorName;
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
    public string CreatorName { get { return _creatorName; } set { _creatorName = value; } }
    
    public  IReadOnlyList<Equipement> Equipements { get { return _equipements; } }

    public BiodomeEtat Etat => BiodomeEtatCompanion.DeterminerEtat(this);

    public bool DeepEquals(Object other)
    {
        if (!(other is Biodome))
        {
            return false;
        }

        Biodome otherBiodome = (Biodome)other;

        if (otherBiodome.Id == this.Id 
            && otherBiodome.Image == this.Image 
            && otherBiodome.CreatorName == this.CreatorName 
            && otherBiodome.Nom == this.Nom
            && otherBiodome.Temperature == this.Temperature
            && otherBiodome.Humidite == this.Humidite)
        {
            return true;
        }

        return false;
    }
}