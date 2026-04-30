using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Core;

public class Species(
    string nom,
    double taille,
    double poids,
    DietType dietType,
    SpeciesClassification speciesClassification,
    string avatar,
    bool visible,
    string ownerName,
    int quantity)
{
    public Species(string nom, double taille, double poids,  DietType dietType,  SpeciesClassification speciesClassification, string avatar, bool visible, string ownerName) : this(nom, taille, poids, dietType, speciesClassification, avatar, visible, ownerName, 0)
    {
    }

    public override bool Equals(object? other)
    {
        if (other == null || !(other is Species))
        {
            return false;
        }
        
        Species otherSpecies = (Species)other;

        return otherSpecies.Nom == this.Nom 
               &&  Math.Abs(otherSpecies.Taille - this.Taille) < 0.001 
               && Math.Abs(otherSpecies.Poids - this.Poids) < 0.001 
               && otherSpecies.DietType == this.DietType 
               && otherSpecies.SpeciesClassification == this.SpeciesClassification;
    }

    public string Nom => nom;

    public string GetSlug()
    {
        return nom.ToKebabCase();
    }
    
    public double Taille => taille;

    public double Poids => poids;

    public DietType DietType => dietType;

    public SpeciesClassification SpeciesClassification => speciesClassification;

    public string Avatar { get; set; } = avatar;

    public bool IsVisible { get; set; } = visible;

    public string OwnerName => ownerName;

    public int Quantity { get; set; } = quantity;
}