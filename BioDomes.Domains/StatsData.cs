using BioDomes.Domains.Core;

namespace BioDomes.Domains;

public record StatsData(
    Biodome Biodome,

    // chart 1
    Dictionary<string, List<Species>> QuantityByClassification,
    // chart 2
    Dictionary<string, List<Species>> QuantityByRegim
    
)
{
}
