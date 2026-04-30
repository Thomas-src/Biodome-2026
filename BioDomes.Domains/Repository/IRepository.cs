using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;

namespace BioDomes.Domains.Extensions;

public interface IRepository
{
    IList<Species> GetallEspece();
    IList<Biodome> GetBestThreeBiodome();
    void Save(Species e);
    IList<Biodome> GetAllDomes();
}