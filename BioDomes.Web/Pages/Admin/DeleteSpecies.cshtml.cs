using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

public class DeleteSpeciesModel : PageModel
{
    private readonly ISpeciesService _speciesService;
    private readonly IBiodomeService _biodomesService;

    public DeleteSpeciesModel(ISpeciesService speciesService, IBiodomeService biodomesService)
    {
        _speciesService = speciesService;
        _biodomesService = biodomesService;
    }

    public Domains.Core.Species Species { get; set; } = default!;

    public IActionResult OnGet(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return NotFound();

        var species = _speciesService.Get(slug);

        if (species is null)
        {
            return NotFound();
        }

        Species = species;
        return Page();
    }

    public IActionResult OnPostDelete(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return NotFound();

        var usageCount =  _speciesService.Get(slug);
        var biodomesHasSpecies = _biodomesService.HasSpecies(slug);
        if (biodomesHasSpecies.Count == 0)
        {
            _speciesService.Delete(slug);
            TempData["SuccessMessage"] = "Espèce supprimée avec succès";
            return RedirectToPage("/Admin/species");
        }
        else
        {
            
        }
      

        return Page();
    }
}