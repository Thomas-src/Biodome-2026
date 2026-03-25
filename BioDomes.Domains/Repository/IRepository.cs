using BioDomes.Domains;
using BioDomes.Domains.Core;

namespace BioDomes.Domains.Extensions;

public interface IRepository
{
    IList<Espece> GetallEspece();
    IList<Biodome> GetBestThreeBiodome();
    void Save(Espece e);
}