using BioDomes.Domains.Core;
using BioDomes.Domains.Services.interfaces;

namespace BioDomes.Domains.Services;


/**
 * Générée par ia (claude), service pour la création de graphiques
 */
public class FoodChainService : IFoodChainService
{
    public FoodChain Build(IReadOnlyList<Species> species)
    {

        var nodes = BuildNodes(species);
        var links = BuildLinks(species);

        AssignPositions(nodes);

        return new FoodChain { Nodes = nodes, Links = links };
    }

    // ── Nodes ────────────────────────────────────────────────────────────────

    private List<FoodChainNode> BuildNodes(IReadOnlyList<Species> species)
    {
        return species.Select(s => new FoodChainNode
        {
            Id           = s.GetSlug(),
            Label        = s.Nom,
            Color        = s.DietType.GetColor(),
            TrophicLevel = s.DietType.GetTrophicLevel(),
            Image = s.Avatar,
            Radius       = GetRadius(s.Poids)
        }).ToList();
    }

    private List<FoodChainLink> BuildLinks(IReadOnlyList<Species> species)
    {
        var links = new List<FoodChainLink>();
        foreach (var predator in species)
        {
            var edible = NutrientChain.CanEat(predator.DietType);
            foreach (var prey in species)
            {
                if (prey.Nom != predator.Nom && edible.Contains(prey.SpeciesClassification))
                {
                    links.Add(new FoodChainLink
                    {
                        From = prey.GetSlug(),
                        To   = predator.GetSlug()
                    });
                }
            }
        }
        return links;
    }

    // ── Layout ───────────────────────────────────────────────────────────────

    private static void AssignPositions(List<FoodChainNode> nodes)
    {
        // Regrouper par niveau trophique
        var byLevel = nodes
            .GroupBy(n => n.TrophicLevel)
            .OrderBy(g => g.Key)
            .ToList();

        var totalLevels = byLevel.Count;

        for (var li = 0; li < byLevel.Count; li++)
        {
            var group    = byLevel[li].ToList();
            var count    = group.Count;
            var yRatio = totalLevels == 1 ? 0.5 : 0.1 + (li / (double)(totalLevels - 1)) * 0.82;

            for (var i = 0; i < count; i++)
            {
                var xRatio = count == 1 ? 0.5 : 0.1 + (i / (double)(count - 1)) * 0.82;
                group[i].X = Math.Round(xRatio, 4);
                group[i].Y = Math.Round(yRatio, 4);
            }
        }
    }
    
    private static int GetRadius(double poids) => poids switch
    {
        < 50    => 16,
        < 500   => 20,
        < 5000  => 24,
        < 50000 => 28,
        _       => 32
    };
}
public class FoodChainNode
{
    public string Id { get; set; } = "";
    public string Label { get; set; } = "";
    public string Color { get; set; } = "";
    public double X { get; set; }
    public double Y { get; set; }
    public int Radius { get; set; }
    public int TrophicLevel { get; set; }
    public string Image { get; set; }
}

public class FoodChainLink
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
}

public class FoodChain
{
    public List<FoodChainNode> Nodes { get; set; } = new();
    public List<FoodChainLink> Links { get; set; } = new();
}