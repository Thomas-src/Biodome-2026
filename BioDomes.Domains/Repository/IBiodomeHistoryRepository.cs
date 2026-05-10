using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiodomeHistoryRepository
{
    public void Save(int biodomeId, int speciesId, int quantity);
    public void Remove(int biodomeId, int speciesId);
    public IReadOnlyList<BiodomeSpeciesHistory> GetHistory(string biodomeSlug, string username);
    public void DeleteHistory(int biodomeId);
}