using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IHistoryService
{
  
    public SpeciesEvolutionData Buid(string biodomeSlug, string username);
}