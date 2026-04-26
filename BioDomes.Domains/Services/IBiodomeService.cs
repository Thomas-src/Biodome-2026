using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface IBiodomeService
{
    IReadOnlyList<Biodome> GetBestThreeBiodome();
    IReadOnlyList<Biodome> GetAllBiodomes();
    void Save(Biodome created, Stream openReadStream);

    double CalculateHumiditeMax(double temp);
}