using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

public class SpeciesAdminModel : PageModel
{
    private readonly IBiodomeService _biodomesService;
    private readonly ISpeciesService _speciesService;
    private readonly ILogger<SpeciesAdminModel> _logger;

    public Dictionary<string, int> SpeciesBiodomeCount { get; set; } = new();
    public IReadOnlyList<Domains.Core.Species> SpeciesVerified { get; set; } = [];
    public IReadOnlyList<Domains.Core.Species> SpeciesToVerify { get; set; } = [];

    public SpeciesAdminModel(ISpeciesService speciesService, IBiodomeService biodomesService, ILogger<SpeciesAdminModel> logger)
    {
        _speciesService = speciesService;
        _biodomesService = biodomesService;
        _logger = logger;
    }

    public void OnGet()
    {
        SpeciesVerified = _speciesService.GetAvailableSpecies();
        SpeciesToVerify = _speciesService.GetUnavailableSpecies();
        
        
        var allSpecies = SpeciesVerified.Concat(SpeciesToVerify).ToList();
        SpeciesBiodomeCount = allSpecies.ToDictionary(
            s => s.Nom,
            s => _biodomesService.HasSpecies(s.Nom).Count
        );
    }
    
    public IActionResult OnPostMakePublic(string name)
    {
        _logger.LogInformation("post pour {}", name);
        _speciesService.MakePublic(name);

        return RedirectToPage();
    }
}