using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IHistoryService
{
    public IReadOnlyList<BiodomeSpeciesHistory> BuildHistory(int biodomeId, string username);
    
}