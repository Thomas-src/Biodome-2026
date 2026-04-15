using BioDomes.Domains;
using BioDomes.Domains.Core;

namespace BioDomes.Domains.Extensions;

public interface IRepository
{
    IList<Species> GetallEspece();
    IList<Biodome> GetBestThreeBiodome();
    void Save(Species e);
    IList<Biodome> GetAllDomes();
}