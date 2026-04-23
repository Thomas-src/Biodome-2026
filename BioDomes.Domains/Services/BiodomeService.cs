using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;

namespace BioDomes.Domains.Services;

public class BiodomeService : IBiodomeService
{
    private IBiodomeRepository _repository;
    public BiodomeService(IBiodomeRepository repository)
    {
        _repository = repository;
    }
    public IReadOnlyList<Biodome> GetBestThreeBiodome()  // todo nb espèces le plus haut
        => _repository.GetAllBiodomes().OrderByDescending(b => b.Etat).Take(3).ToList();
    

    public IReadOnlyList<Biodome> GetAllBiodomes()
    => _repository.GetAllBiodomes();
    
}