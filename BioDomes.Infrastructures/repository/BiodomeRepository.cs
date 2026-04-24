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
        return _context.Biodomes
            .Select(b => new Biodome(
                b.idBiodomes,
                b.Name,
                b.Temperature,
                b.Humidity,
                b.Avatar,
                b.Species.Select(s => new Species(
                    s.Name,
                    s.Size,
                    s.Weight,
                    Enum.Parse<DietType>(s.Regim.Name),
                    Enum.Parse<SpeciesClassification>(s.TypeEntity.Name),
                    s.Avatar,
                    s.Visible,
                    s.User != null ? s.User.UserName : "unknown"
                )).ToList()
            ))
            .ToList();
    }
    public void Create(Biodome biodome)
    {
        var entity = new BiodomeEntity
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