namespace BioDomes.Domains.Core;

public class Species
{
    private DietType _dietType;
    private SpeciesClassification _speciesClassification;

    private string _nom;
    private double _taille;
    private double _poids;
    private string _avatar;
    private bool _IsVisible;

    private string _ownerName;

    public Species(string nom, double taille, double poids,  DietType dietType,  SpeciesClassification speciesClassification, string avatar, bool visible, string ownerName)
    {
        this._nom = nom;
        this._taille = taille;
        this._poids = poids;
        this._dietType = dietType;
        this._speciesClassification = speciesClassification;
        this._avatar = avatar;
        this._IsVisible = visible;
        this._ownerName = ownerName;
    }

    public override bool Equals(Object other)
    {
        if (other == null || !(other is Species))
        {
            return false;
        }
        
        Species otherSpecies = (Species)other;

        if (otherSpecies.Nom == this.Nom 
            &&  otherSpecies.Taille == this.Taille 
            && otherSpecies.Poids == this.Poids 
            && otherSpecies.DietType == this.DietType 
            && otherSpecies.SpeciesClassification == this.SpeciesClassification)
        {
            return true;
        }

        return false;
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
        set { _avatar = value; }
    }

    public bool IsVisible
    {
        get { return _IsVisible; }
        set  { _IsVisible = value; }
    }

    public string OwnerName{
        get { return _ownerName; }
    }
}