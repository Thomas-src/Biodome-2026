namespace BioDomes.Domains.Core;

public class Biodome(string nom, double temperature, double humidite, IList<Espece> especes)
{
    
    public string Nom = nom;
    public IList<Espece> _especes = especes;
    
    public double Temperature = temperature;
    public double Humidite = humidite;
    
    public BiodomeEtat Etat = BiodomeEtatCompanion.DeterminerEtat(temperature,humidite); 

}