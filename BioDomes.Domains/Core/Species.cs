namespace BioDomes.Domains.Core;

public class Species
{
    private DietType _dietType;
    private SpeciesClassification _speciesClassification;

    private string _nom;
    private double _taille;
    private double _poids;
    private string _avatar;

    public Species(string nom, double taille, double poids,  DietType dietType,  SpeciesClassification speciesClassification, string avatar)
    {
        this._nom = nom;
        this._taille = taille;
        this._poids = poids;
        this._dietType = dietType;
        this._speciesClassification = speciesClassification;
        this._avatar = avatar;
    }

    public string Nom
    {
        get { return _nom; }
    }

    public double Taille
    {
        get { return _taille; }
    }

    public double Poids
    {
        get { return _poids; }
    }

    public DietType DietType
    {
        get { return _dietType; }
    }

    public SpeciesClassification SpeciesClassification
    {
        get { return _speciesClassification; }
    }

    public string Avatar{
        get { return _avatar; }
    }
}