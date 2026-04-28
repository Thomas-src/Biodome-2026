using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface IBiodomeService
{
    IReadOnlyList<Biodome> GetBestThreeBiodome();
    IReadOnlyList<Biodome> GetAllBiodomes();
    
    IReadOnlyList<Biodome> SearchBiodomes(
        double? temperatureMin, double? temperatureMax,
        double? humiditeMin, double? humiditeMax,
        string? equipement, string? espece);
    void Save(Biodome created, Stream openReadStream);

    double CalculateHumiditeMax(double temp);
    Biodome GetBiodomeById(int biodomeId);
}