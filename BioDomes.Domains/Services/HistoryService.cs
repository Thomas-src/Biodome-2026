using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services.interfaces;

namespace BioDomes.Domains.Services;

public class HistoryService(IBiodomeHistoryRepository biodomeSpeciesHistory) : IHistoryService
{
    public IReadOnlyList<BiodomeSpeciesHistory> Build(string biodomeSlug, string username) =>
        biodomeSpeciesHistory.GetHistory(biodomeSlug, username);

}