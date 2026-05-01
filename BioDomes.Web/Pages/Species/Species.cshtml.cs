using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Species;

public class SpeciesModel : PageModel
{
    private readonly ISpeciesRepository _repository;
    private readonly ISpeciesService _speciesService;

    private IReadOnlyList<Domains.Core.Species> _ownSpecies;

    public IReadOnlyList<Domains.Core.Species> OwnSpecies
    {
        get { return _ownSpecies; }
        set { _ownSpecies = value; }
    }

    private IReadOnlyList<Domains.Core.Species> _otherSpecies;

    public IReadOnlyList<Domains.Core.Species> OtherSpecies
    {
        get { return _otherSpecies; }
        set { _otherSpecies = value; }
    }

    public SpeciesModel(ISpeciesRepository repository, ISpeciesService speciesService)
    {
        _repository = repository;
        _speciesService = speciesService;
    }

    public void OnGet()
    {
        try
        {
            IReadOnlyList<Domains.Core.Species> speciesList = _speciesService.GetAllSpeices();

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