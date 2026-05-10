using BioDomes.Domains.Extensions;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

[ValidateAntiForgeryToken]
public class SpeciesAdminModel : PageModel
{
    private readonly IBiodomeService _biodomesService;
    private readonly ISpeciesService _speciesService;
    private readonly ILogger<SpeciesAdminModel> _logger;

    public Dictionary<(string Nom, string Owner), int> SpeciesBiodomeCount { get; set; } = new();
    public IReadOnlyList<Domains.Core.Species> SpeciesVerified { get; set; } = [];
    public IReadOnlyList<Domains.Core.Species> SpeciesToVerify { get; set; } = [];

    public SpeciesAdminModel(ISpeciesService speciesService, IBiodomeService biodomesService,
        ILogger<SpeciesAdminModel> logger, UserManager<UserEntity> userManager)
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
            s => (s.Nom, s.OwnerName),
            s => _biodomesService.HasSpecies(s.Nom, s.OwnerName).Count
        );
    }

    public IActionResult OnPostMakePublic(string name, string ownerName)
    {
        if (_speciesService.MakePublic(name.ToKebabCase(), ownerName))
        {
            TempData["SuccessMessage"] = $"Espèce {name} de {ownerName} rendue publique avec succès";
        }
        else
        {
            TempData["ErrorMessage"] = $"Impossible de rendre {name} publique car elle a été supprimée ou l'espèce d'un autre utilisateur a déjà été validée.";
        }
        
        return RedirectToPage();
    }
}