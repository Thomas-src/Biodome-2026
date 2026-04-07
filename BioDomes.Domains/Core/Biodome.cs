namespace BioDomes.Domains.Core;

public record Biodome(
    string Id,
    string Nom,
    double Temperature,
    double Humidite,
    string Image
)
{
    public BiodomeEtat Etat => BiodomeEtatCompanion.DeterminerEtat(Temperature, Humidite);
}