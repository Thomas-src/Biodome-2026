namespace BioDomes.Domains.Core;

public record Biodome(
    string Id,
    string Nom,
    double Temperature,
    double Humidite,
    IList<Espece> Especes,
    string Image
)
{
    public BiodomeEtat Etat => BiodomeEtatCompanion.DeterminerEtat(Temperature, Humidite);
}