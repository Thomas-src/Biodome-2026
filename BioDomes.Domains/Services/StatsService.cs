using System.Collections.ObjectModel;
using BioDomes.Domains.Core;
using BioDomes.Domains.Services.interfaces;

namespace BioDomes.Domains.Services;

public class StatsService(IFoodChainService foodChainService) : IStatsService
{
    public StatsData GetBiodomeStats(Biodome biodome)
    {
        var species = biodome.Species;
        return new StatsData(biodome, GetNbByClassification(species), GetNbByRegim(species),foodChainService.Build(species));
    }

    private static Dictionary<string, List<Species>> GetNbByClassification(IReadOnlyList<Species> species)
        => species
            .GroupBy(s => s.SpeciesClassification.ToString())
            .ToDictionary(group => group.Key, group => group.ToList());

    private static Dictionary<string, List<Species>> GetNbByRegim(IReadOnlyList<Species> species)
        => species
            .GroupBy(s => s.DietType.ToString())
            .ToDictionary(group => group.Key, group => group.ToList());
}