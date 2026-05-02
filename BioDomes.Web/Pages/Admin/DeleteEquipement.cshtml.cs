using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

[ValidateAntiForgeryToken]
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

    public IActionResult OnGet(string slug, string ownerName)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var equipement = _service.GetByKebabCase(slug, ownerName);

        if (equipement is null)
        {
            return NotFound();
        }

        Equipement = equipement;
        return Page();
    }

    
    public IActionResult OnPostDelete(string slug, string ownerName)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var biodomesHasSpecies = _biodomesService.HasEquipement(slug, ownerName);
        if (biodomesHasSpecies.Count == 0)
        {
            _service.Delete(slug, ownerName);
            TempData["SuccessMessage"] = "Équipement supprimé avec succès";
            return RedirectToPage("/Admin/Equipements");
        }
        else
        {
        }

        return Page();
    }
}