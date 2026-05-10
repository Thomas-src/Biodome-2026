using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services.interfaces;

namespace BioDomes.Domains.Services;

public class HistoryService(IBiodomeHistoryRepository biodomeSpeciesHistory) : IHistoryService
{

    public SpeciesEvolutionData Buid(string biodomeSlug, string username)
    {
        var histories = biodomeSpeciesHistory.GetHistory(biodomeSlug, username)
            .OrderBy(h => h.UpdateAt)
            .ToList();

        var allDates = histories
            .Select(h => h.UpdateAt.ToString("dd/MM/yyyy"))
            .Distinct()
            .Order()
            .ToList();

        var runningTotals = new Dictionary<string, int>();
        var evolutionBySpecies = new Dictionary<string, Dictionary<string, int>>();

        foreach (var history in histories)
        {
            var name = history.Species.Nom;
            var date = history.UpdateAt.ToString("dd/MM/yyyy");

            runningTotals.TryAdd(name, 0);
            evolutionBySpecies.TryAdd(name, []);

            runningTotals[name] += history.Delta;
            evolutionBySpecies[name][date] = runningTotals[name];
        }

        var quantitiesBySpecies = new Dictionary<string, List<int>>();

        foreach (var (speciesName, dateValues) in evolutionBySpecies)
        {
            var lastValue = 0;
            quantitiesBySpecies[speciesName] = allDates.Select(date =>
            {
                if (dateValues.TryGetValue(date, out var value))
                    lastValue = value;
                return lastValue;
            }).ToList();
        }

        return new SpeciesEvolutionData
        {
            Dates = allDates,
            QuantitiesBySpecies = quantitiesBySpecies
        };
    }
}