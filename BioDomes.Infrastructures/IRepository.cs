using BioDomes.Domains;

namespace BioDomes.Infrastructures;

public interface IRepository
{
    IList<Espece> GetallEspece();
}