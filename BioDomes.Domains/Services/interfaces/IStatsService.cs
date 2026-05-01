using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IStatsService {
    public StatsData GetBiodomeStats(Biodome biodome);
}