namespace BioDomes.Domains;

public class Espece
{
    private string _type;
    private string _nom;
    private string _regime;
    private int _taille;

    public Espece(string type, string nom, string regime, int taille)
    {
        _type = type;
        _nom = nom;
        _regime = regime;
        _taille = taille;
    }

    public string Type => _type;
    public string Nom => _nom;
    public string Regime => _regime;
    public int Taille => _taille;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        if (obj == this)
        {
            return true;
        }

        Espece other = (Espece)obj;

        return _type == other._type
               && _nom == other._nom
               && _regime == other._regime
               && _taille == other._taille;
    }
}