namespace BioDomes.Domains;

public class Espece
{
    private string _type;
    private string _regime;
    private int taille;

    public Espece(string type, string regime, int taille)
    {
        _type = type;
        _regime = regime;
    }

    public string Type => _type;
    public string Regime => _regime;
    public int Taille => taille;

}