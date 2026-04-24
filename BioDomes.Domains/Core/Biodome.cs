namespace BioDomes.Domains.Core;

public record Biodome(
    int Id,
    string Nom,
    double Temperature,
    double Humidite,
    string Image,
    IReadOnlyList<Species> Species)
{
    public BiodomeEtat Etat => BiodomeEtatCompanion.DeterminerEtat(Temperature, Humidite);
}