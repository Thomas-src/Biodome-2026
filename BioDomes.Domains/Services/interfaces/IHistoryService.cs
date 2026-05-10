using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IHistoryService
{
    public IReadOnlyList<BiodomeSpeciesHistory> Build(string biodomeSlug, string username);
    
}