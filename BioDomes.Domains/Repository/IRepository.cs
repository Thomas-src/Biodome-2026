using BioDomes.Domains;
using BioDomes.Domains.Core;

namespace BioDomes.Domains.Extensions;

public interface IRepository
{
    IList<Espece> GetallEspece();
    void Save(Espece e);
}