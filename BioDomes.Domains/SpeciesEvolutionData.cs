namespace BioDomes.Domains;

public class SpeciesEvolutionData
{
    public List<string> Dates { get; set; } = [];
    public Dictionary<string, List<int>> QuantitiesBySpecies { get; set; } = [];
}