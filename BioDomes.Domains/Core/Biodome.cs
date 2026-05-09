using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Core;

public class Biodome
{
    public Biodome(string nom, double temperature, double humidite, string image,IReadOnlyList<Species> species, IReadOnlyList<Equipement> equipements, string creatorName, string slug)
    {
        Nom = nom;
        Temperature = temperature;
        Humidite = humidite;
        Image = image;
        Species = species;
        Equipements = equipements;
        CreatorName = creatorName;
        Slug = slug;
    }
    
    public string Slug { get; }
    public IReadOnlyList<Species> Species { get; }
    public string Nom { get; }
    public double Temperature { get; }
    public double Humidite { get; }
    public string Image { get; set; }

    public string CreatorName { get; set; }

    public  IReadOnlyList<Equipement> Equipements { get; }

    public BiodomeEtat Etat => BiodomeEtatCompanion.DeterminerEtat(this);

    public bool DeepEquals(object other)
    {
        if (!(other is Biodome otherBiodome))
        {
            return false;
        }

        return otherBiodome.Image == this.Image 
               && otherBiodome.CreatorName == this.CreatorName 
               && otherBiodome.Nom == this.Nom
               && Math.Abs(otherBiodome.Temperature - this.Temperature) < 0.001
               && Math.Abs(otherBiodome.Humidite - this.Humidite) < 0.001;
    }
}