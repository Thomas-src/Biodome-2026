using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;

namespace BioDomes.Infrastructures.repository;

public class BiodomeRepository : IBiodomeRepository
{
    
    private readonly BioDomesDbContext _context;
    
    public BiodomeRepository(BioDomesDbContext context)
    {
        _context = context;
    }
    
   /*
    * public IReadOnlyList<Biodome> GetBestThreeBiodome()
    {
        return _context.Biodomes
            .Select(b => new Biodome(b.idBiodomes, b.Name, b.Temperature, b.Humidity, b.Avatar))
            .OrderByDescending(b => b.Etat)
            .Take(3)
            .ToList<Biodome>();

    }
    */

    public IReadOnlyList<Biodome> GetAllBiodomes()
    {
        return _context.Biodomes.Select(b => new Biodome(b.idBiodomes, b.Name, b.Temperature, b.Humidity, b.Avatar)).ToList<Biodome>();
    }

    public void Create(Biodome biodome)
    {
        var entity = new Biodomes
        {
            idBiodomes = biodome.Id,
            Name = biodome.Nom,
            Temperature = biodome.Temperature,
            Humidity = biodome.Humidite,
            Avatar = biodome.Image
        };
        _context.Biodomes.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Biodome biodome)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }
}