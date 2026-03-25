using BioDomes.Domains;

namespace BioDomes.Domains.Extensions;

public interface IRepository
{
    IList<Espece> GetallEspece();
    void Save(Espece e);
}