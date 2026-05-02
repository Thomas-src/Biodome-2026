using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

public class Details : PageModel
{
    private readonly IBiodomeService _service;
    private readonly IEquipementRepository _equipementRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IStatsService _statsService;
    public Biodome Biodome { get; set; }

    public bool Owner { get; set; } = false;
    public IReadOnlyList<Domains.Core.Species> AvailableSpecies { get; set; }
    public IReadOnlyList<Domains.Core.Equipements.Equipement> AvailableEquipements { get; set; }

    // chart 1
    public List<string> ClassificationLabels { get; set; } = [];
    public List<int> ClassificationCounts { get; set; } = [];
    public Dictionary<string, List<SpeciesChartDto>> ClassificationTooltipData { get; set; } = new();

    // chart 2
    public List<string> RegimeLabels { get; set; } = [];
    public List<int> RegimeCounts { get; set; } = [];
    public Dictionary<string, List<SpeciesChartDto>> RegimeTooltipData { get; set; } = new();

    public Details(IBiodomeService service, ISpeciesRepository speciesRepository,
        IEquipementRepository equipementRepository, IStatsService statsService)
    {
        _service = service;
        _speciesRepository = speciesRepository;
        _equipementRepository = equipementRepository;
        _statsService = statsService;
    }

    public IActionResult OnGet(int id)
    {
        var biodome = _service.GetBiodomeById(id);

        if (biodome == null)
        {
            return NotFound();
        }
        else
        {
            Biodome = biodome;
        }

        if (User.Identity.IsAuthenticated)
        {
            if (Biodome.CreatorName == User.Identity.Name)
            {
                Owner = true;
                AvailableEquipements = _equipementRepository.GetAllEquipements().Where(x => x.Visibility || x.OwnerName == Biodome.CreatorName).ToList();
                AvailableSpecies = _speciesRepository.GetAllSpecies().Where(x => x.IsVisible || x.OwnerName == Biodome.CreatorName).ToList();
                // todo afficher les stats
                var stats = _statsService.GetBiodomeStats(biodome);

                // chart 1
                ClassificationLabels = stats.QuantityByClassification.Keys.ToList();

                ClassificationCounts = stats.QuantityByClassification
                    .Values.Select(v => v.Count).ToList();

                ClassificationTooltipData = stats.QuantityByClassification
                    .ToDictionary(x => x.Key, x => x.Value
                        .Select(s => new SpeciesChartDto(s.Nom, s.Quantity)).ToList()
                    );
                // chart 2 
                RegimeLabels = stats.QuantityByRegim.Keys.ToList();

                RegimeCounts = stats.QuantityByRegim.Values.Select(v => v.Count).ToList();
                RegimeTooltipData = stats.QuantityByRegim
                    .ToDictionary(x => x.Key, x => x.Value
                        .Select(s => new SpeciesChartDto(s.Nom, s.Quantity)).ToList()
                    );
                
            }
        }


        return Page();
    }

    public IActionResult OnPostAddSpecies(int biomeId, string speciesName, int quantity = 1)
    {
        _service.AddSpeciesTo(biomeId, speciesName, quantity);

        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostAddEquipement(int biomeId, string equipementName)
    {
        _service.AddEquipementTo(biomeId, equipementName);

        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostRemoveSpecies(int biomeId, string speciesName)
    {
        _service.RemoveSpeciesFrom(biomeId, speciesName);
        // TempData["SuccessMessage"] = $"L'espèce {speciesName} a été retirée du biodome.";
        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostRemoveEquipement(int biomeId, string equipementName)
    {
        _service.RemoveEquipementFrom(biomeId, equipementName);
        // TempData["SuccessMessage"] = $"L'équipement {equipementName} a été retiré du biodome.";
        return RedirectToPage(new { id = biomeId });
    }

    public record SpeciesChartDto(
        string Name,
        int Quantity
    );
}