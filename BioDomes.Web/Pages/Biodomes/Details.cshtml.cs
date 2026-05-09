using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

[ValidateAntiForgeryToken]
public class DetailsModel : PageModel
{
    private readonly IBiodomeService _service;
    private readonly IEquipementRepository _equipementRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IStatsService _statsService;
    private readonly ILogger<DetailsModel> _logger;
    public Biodome Biodome { get; set; }

    public bool Owner { get; set; } = false;
    public IReadOnlyList<Domains.Core.Species> AvailableSpecies { get; set; } = new List<Domains.Core.Species>();

    public IReadOnlyList<Domains.Core.Equipements.Equipement> AvailableEquipements { get; set; } =
        new List<Domains.Core.Equipements.Equipement>();

    // chart 1
    public List<string> ClassificationLabels { get; set; } = [];
    public List<int> ClassificationCounts { get; set; } = [];
    public Dictionary<string, List<SpeciesChartDto>> ClassificationTooltipData { get; set; } = new();

    // chart 2
    public List<string> RegimeLabels { get; set; } = [];
    public List<int> RegimeCounts { get; set; } = [];
    public Dictionary<string, List<SpeciesChartDto>> RegimeTooltipData { get; set; } = new();

    // chart 3
    public FoodChain FoodChain { get; set; } = new();

    public DetailsModel(IBiodomeService service, ISpeciesRepository speciesRepository,
        IEquipementRepository equipementRepository, IStatsService statsService, ILogger<DetailsModel> logger)
    {
        _service = service;
        _speciesRepository = speciesRepository;
        _equipementRepository = equipementRepository;
        _statsService = statsService;
        _logger = logger;
    }

    public IActionResult OnGet(string id)
    {
        _logger.LogInformation("Getting details for {id}", id);
        var biodome = _service.GetBiodomeBySlug(id);

        if (biodome == null)
        {
            _logger.LogInformation("No biodome found for {id}", id);
            return NotFound();
        }

        Biodome = biodome;

        LoadStats();

        return Page();
    }

    private void LoadStats()
    {
        if (User.Identity.IsAuthenticated && Biodome.CreatorName == User.Identity.Name)
        {
            Owner = true;
            AvailableEquipements = _equipementRepository.GetAllEquipements()
                .Where(x => x.Visibility || x.OwnerName == Biodome.CreatorName).ToList();
            AvailableSpecies = _speciesRepository.GetAllSpecies()
                .Where(x => x.IsVisible || x.OwnerName == Biodome.CreatorName).ToList();
            var stats = _statsService.GetBiodomeStats(Biodome);

            // chart 1
            ClassificationLabels = stats.QuantityByClassification.Keys.ToList();
            ClassificationCounts = stats.QuantityByClassification.Values.Select(v => v.Count).ToList();

            ClassificationTooltipData = stats.QuantityByClassification.ToDictionary(x => x.Key, x => x.Value
                .Select(s => new SpeciesChartDto(s.Nom, s.Quantity)).ToList()
            );
            // chart 2 
            RegimeLabels = stats.QuantityByRegim.Keys.ToList();

            RegimeCounts = stats.QuantityByRegim.Values.Select(v => v.Count).ToList();
            RegimeTooltipData = stats.QuantityByRegim.ToDictionary(x => x.Key, x => x.Value
                .Select(s => new SpeciesChartDto(s.Nom, s.Quantity)).ToList()
            );
            // chart 3
            FoodChain = stats.Foodchain;
        }
    }


    public IActionResult OnPostAddSpecies(string biomeId, string speciesName, int quantity = 1)
    {
        _service.AddSpeciesTo(biomeId, speciesName, quantity);

        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostAddEquipement(string biomeId, string equipementName)
    {
        _service.AddEquipementTo(biomeId, equipementName);

        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostRemoveSpecies(string biomeId, string speciesName)
    {
        _service.RemoveSpeciesFrom(biomeId, speciesName);
        // TempData["SuccessMessage"] = $"L'espèce {speciesName} a été retirée du biodome.";
        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostRemoveEquipement(string biomeId, string equipementName)
    {
        _service.RemoveEquipementFrom(biomeId, equipementName);
        // TempData["SuccessMessage"] = $"L'équipement {equipementName} a été retiré du biodome.";
        return RedirectToPage(new { id = biomeId });
    }

    public record SpeciesChartDto(
        string Name,
        int Quantity
    );

    public IActionResult OnPostDelete(string biomeId)
    {
        try
        {
            _service.DeleteBiodome(biomeId);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return Page();
        }

        TempData["SuccessMessage"] = $"Le biodome à bien été supprimé !";
        return RedirectToPage("Biodomes");
    }
}