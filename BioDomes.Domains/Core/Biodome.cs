namespace BioDomes.Domains.Core;

public class Biodome(string nom)
{
    private string _nom = nom;
    

    public string Nom => _nom;
}