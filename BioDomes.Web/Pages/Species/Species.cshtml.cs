using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Species;

public class SpeciesModel : PageModel
{
    private readonly ISpeciesService _speciesService;

    public IReadOnlyList<Domains.Core.Species> OwnSpecies { get; set; }

    public IReadOnlyList<Domains.Core.Species> OtherSpecies { get; set; }

    public SpeciesModel(ISpeciesService speciesService)
    {
        _speciesService = speciesService;
    }

    public void OnGet()
    {
        try
        {
            var speciesList = _speciesService.GetAllSpeices();

            if (User.Identity.IsAuthenticated)
            {
                OwnSpecies = _speciesService.FilterOwnerSpecies(User.Identity.Name, speciesList);
                OtherSpecies = _speciesService.FilterSpecies(User.Identity.Name, speciesList);
            }
            else
            {
                OtherSpecies = speciesList.Where(s => s.IsVisible).ToList();
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
        }
    }
}