using BioDomes.Domains;
using BioDomes.Domains.Core;

namespace BioDomes.Infrastructures;

public interface IRepository
{
    IList<Espece> GetallEspece();
    void Save(Espece e);
}