using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiodomeRepository
{
    
    IReadOnlyList<Biodome> GetBestThreeBiodome();
    
    IReadOnlyList<Biodome> GetAllBiodomes();
    
}