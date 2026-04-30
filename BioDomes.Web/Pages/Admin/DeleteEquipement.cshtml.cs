using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

public class DeleteEquipementModel : PageModel
{
    private readonly IEquipementService _service;
    private readonly IBiodomeService _biodomesService;

    public DeleteEquipementModel(IEquipementService equipementService, IBiodomeService biodomesService)
    {
        _service = equipementService;
        _biodomesService = biodomesService;
    }

    public Domains.Core.Equipements.Equipement Equipement { get; set; }

    public IActionResult OnGet(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return NotFound();

        var equipement = _service.Get(slug);

        if (equipement is null)
        {
            return NotFound();
        }

        Equipement = equipement;
        return Page();
    }

    public IActionResult OnPostDelete(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return NotFound();

        var usageCount =  _service.Get(slug);
        var biodomesHasSpecies = _biodomesService.HasEquipement(slug);
        if (biodomesHasSpecies.Count == 0)
        {
            _service.Delete(slug);
            TempData["SuccessMessage"] = "Équipement supprimé avec succès";
            return RedirectToPage("/Admin/Equipements");
        }
        else
        {
            
        }
        
        return Page();
    }
}