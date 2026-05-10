using System.Text.RegularExpressions;
using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
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
    // chart 4
    public SpeciesEvolutionData SpeciesEvolution { get; set; }  = new();

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
        id = SanitizeInput(id).ToSlug();
        try
        {
            var biodome = _service.GetBiodomeBySlug(id);
            Biodome = biodome;
            LoadStats();

            return Page();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "No biodome found for {id}", id);
            return NotFound();
        }
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
            
            // chart 4 
            SpeciesEvolution = stats.SpeciesEvolution;
        }
    }


    public IActionResult OnPostAddSpecies(string biomeId, string speciesName, int quantity = 1)
    {
        var biomeSlug = SanitizeInput(biomeId).ToSlug();
        var species = SanitizeInput(speciesName);

        _service.AddSpeciesTo(biomeSlug, species, quantity);

        return RedirectToPage(new { id = biomeSlug });
    }

    public IActionResult OnPostAddEquipement(string biomeId, string equipementName)
    {
        var biomeSlug = SanitizeInput(biomeId).ToSlug();
        var species = SanitizeInput(equipementName);

        _service.AddEquipementTo(biomeSlug, species);

        return RedirectToPage(new { id = biomeSlug });
    }

    public IActionResult OnPostRemoveSpecies(string biomeId, string speciesName)
    {
        var biomeSlug = SanitizeInput(biomeId).ToSlug();
        var species = SanitizeInput(speciesName);

        _service.RemoveSpeciesFrom(biomeSlug, species);
        return RedirectToPage(new { id = biomeSlug });
    }

    public IActionResult OnPostRemoveEquipement(string biomeId, string equipementName)
    {
        var biomeSlug = SanitizeInput(biomeId).ToSlug();
        _service.RemoveEquipementFrom(biomeSlug, equipementName);
        return RedirectToPage(new { id = biomeSlug });
    }

    public record SpeciesChartDto(
        string Name,
        int Quantity
    );

    public IActionResult OnPostDelete(string biomeId)
    {
        var biomeSlug = SanitizeInput(biomeId);
        try
        {
            _service.DeleteBiodome(biomeSlug);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return Page();
        }

        TempData["SuccessMessage"] = $"Le biodome à bien été supprimé !";
        return RedirectToPage("Biodomes");
    }

    private string SanitizeInput(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
}