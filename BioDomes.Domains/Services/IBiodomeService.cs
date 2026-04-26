using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface IBiodomeService
{
    IReadOnlyList<Biodome> GetBestThreeBiodome();
    IReadOnlyList<Biodome> GetAllBiodomes();
    void SavePicture(Biodome created, string avatarFileName, Stream openReadStream);
    void Save(Biodome created);

    double CalculateHumiditeMax(double temp);
}