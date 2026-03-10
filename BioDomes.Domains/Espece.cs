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

}