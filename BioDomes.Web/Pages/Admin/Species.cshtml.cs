using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

public class SpeciesAdminModel : PageModel
{
    private readonly IBiodomeService _biodomesService;
    private readonly ISpeciesService _speciesService;

    public HashSet<string> UnUsedSpeciesNames { get; set; } = [];
    public IReadOnlyList<Domains.Core.Species> SpeciesVerified { get; set; } = [];
    public IReadOnlyList<Domains.Core.Species> SpeciesToVerify { get; set; } = [];

    public SpeciesAdminModel(ISpeciesService speciesService, IBiodomeService biodomesService)
    {
        _speciesService = speciesService;
        _biodomesService = biodomesService;
    }

    public void OnGet()
    {
        SpeciesVerified = _speciesService.GetAvailableSpecies();
        SpeciesToVerify = _speciesService.GetUnavailableSpecies();
        
        
        var allSpecies = SpeciesVerified.Concat(SpeciesToVerify).ToList();
        UnUsedSpeciesNames = allSpecies
            .Where(s => _biodomesService.HasSpecies(s.Nom).Any())
            .Select(s => s.Nom)
            .ToHashSet();
    }
}