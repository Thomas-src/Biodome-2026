using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

public class EquipementsModel : PageModel
{
    private readonly IEquipementService _service;
    private readonly IBiodomeService _biodomeService;
    private readonly ILogger<EquipementsModel> _logger;
    public EquipementsModel(IEquipementService service, IBiodomeService biodomesService,  ILogger<EquipementsModel> logger)
    {
        _service = service;
        _biodomeService = biodomesService;
        _logger = logger;
    }

    public Dictionary<(string Nom, string Owner), int> EquipementsBiodomeCount { get; set; } = new();
    public IReadOnlyList<Domains.Core.Equipements.Equipement> EquipementsVerified { get; set; } = [];
    public IReadOnlyList<Domains.Core.Equipements.Equipement> EquipementsToVerify { get; set; } = [];


    public void OnGet()
    {
        EquipementsVerified = _service.GetAvailableEquipements();
        EquipementsToVerify = _service.GetUnavailableEquipements();


        var allSpecies = EquipementsVerified.Concat(EquipementsToVerify).ToList();
        EquipementsBiodomeCount = allSpecies.ToDictionary(
            s => (s.Name, s.OwnerName),
            s => _biodomeService.HasEquipement(s.Name, s.OwnerName).Count
        );
    }
    
    public IActionResult OnPostMakePublic(string name, string ownerName)
    {
        if ( _service.MakePublic(name, ownerName))
        {
            TempData["SuccessMessage"] = $"L'équipement {name} de {ownerName} a été rendu publique avec succès";
        }
        else
        {
            TempData["ErrorMessage"] = $"Impossible de rendre {name} publique car cet équipement a soit été supprimé ou le même équipement d'autre utilisateur a déjà été validé.";
        }
        
        return RedirectToPage();
    }
}