using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

[ValidateAntiForgeryToken]
public class DeleteSpeciesModel : PageModel
{
    private readonly ISpeciesService _speciesService;
    private readonly IBiodomeService _biodomesService;
    private readonly ILogger<DeleteSpeciesModel> _logger;

    public DeleteSpeciesModel(ISpeciesService speciesService, IBiodomeService biodomesService,
        ILogger<DeleteSpeciesModel> logger)
    {
        _speciesService = speciesService;
        _biodomesService = biodomesService;
        _logger = logger;
    }

    public Domains.Core.Species Species { get; set; }

    public IActionResult OnGet(string slug, string ownerName)
    {
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(ownerName))
        {
            return NotFound();
        }


        var species = _speciesService.GetByKebabCase(slug, ownerName);

        if (species is null)
        {
            return NotFound();
        }

        Species = species;
        return Page();
    }

    public IActionResult OnPostDelete(string slug, string ownerName)
    {
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(ownerName))
        {
            _logger.LogInformation("Slug: or owner is empty {} - {}", slug, ownerName);
            return NotFound();
        }
        slug = slug.ToKebabCase();
        var species = _speciesService.GetByKebabCase(slug, ownerName);
        if (species is null)
        {
            _logger.LogInformation("Species not found for owner: {} - {}", slug, ownerName);
            return NotFound();
        }
        var biodomesHasSpecies = _biodomesService.HasSpecies(species.Nom, ownerName);
        if (biodomesHasSpecies.Count > 0)
        {
            TempData["ErrorMessage"] = "Impossible de supprimer : cette espèce est utilisée dans un ou plusieurs biodômes.";
            return RedirectToPage("/Admin/species");
        }
        _speciesService.Delete(species.Nom, ownerName);
        TempData["SuccessMessage"] = "Espèce supprimée avec succès";
        return RedirectToPage("/Admin/species");
    }
}