using BioDomes.Domains.Core;
using BioDomes.Domains.Services;

namespace BioDomes.Domains;

public record StatsData(
    Biodome Biodome,

    // chart 1
    Dictionary<string, List<Species>> QuantityByClassification,
    // chart 2
    Dictionary<string, List<Species>> QuantityByRegim,
    // chart 3 
    FoodChain Foodchain,
    // chart 4
    SpeciesEvolutionData SpeciesEvolution)
{
}