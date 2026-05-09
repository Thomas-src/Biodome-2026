using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services.interfaces;

namespace BioDomes.Domains.Services;

public class HistoryService(IBiodomeHistoryRepository biodomeSpeciesHistory) : IHistoryService
{
    public IReadOnlyList<BiodomeSpeciesHistory> BuildHistory(int biodomeId, string username) => biodomeSpeciesHistory.GetHistory(biodomeId, username);

}