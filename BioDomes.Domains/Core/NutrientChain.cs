namespace BioDomes.Domains.Core;

/**
 * Cette classe à été générée pas une IA : https://chatgpt.com/c/69f30265-c3f0-83eb-ad23-ef9121ea8620 
 */
public class NutrientChain
{

    public static IList<SpeciesClassification> CanEat(DietType dietType)
    {
        switch (dietType)
        {
            case DietType.Photosynthèse:
                return new List<SpeciesClassification>();

            case DietType.Herbivore:
                return new List<SpeciesClassification>
                {
                    SpeciesClassification.Plantes,
                    SpeciesClassification.Algues,
                    SpeciesClassification.Mousses,
                    SpeciesClassification.Lichen
                };

            case DietType.Carnivore:
                return GetAllAnimals();

            case DietType.Omnivore:
            case DietType.Hétérotrophie:
                return GetAllAnimals().Concat(new List<SpeciesClassification>
                {
                    SpeciesClassification.Plantes,
                    SpeciesClassification.Algues,
                    SpeciesClassification.Mousses,
                    SpeciesClassification.Lichen,
                    SpeciesClassification.Champignons
                }).ToList();

            case DietType.Détritivores:
            case DietType.Coprophage:
            case DietType.Coprophages:
                return GetAllClassifications(); // ils mangent tout (déchets de tout)

            default:
                return new List<SpeciesClassification>();
        }
    }

    private static IList<SpeciesClassification> GetAllAnimals()
    {
        return new List<SpeciesClassification>
        {
            SpeciesClassification.Mammiferes,
            SpeciesClassification.Oiseaux,
            SpeciesClassification.Poissons,
            SpeciesClassification.Reptiles,
            SpeciesClassification.Amphibiens,
            SpeciesClassification.Crustaces,
            SpeciesClassification.Arachnides,
            SpeciesClassification.Insectes,
            SpeciesClassification.Myriapodes,
            SpeciesClassification.Mollusques,
            SpeciesClassification.Echinodermes,
            SpeciesClassification.Anelides,
            SpeciesClassification.Cnidaires,
            SpeciesClassification.Plathelminthes,
            SpeciesClassification.Poriferes
        };
    }

    private static IList<SpeciesClassification> GetAllClassifications()
    {
        return Enum.GetValues(typeof(SpeciesClassification))
                   .Cast<SpeciesClassification>()
                   .ToList();
    }
    
}