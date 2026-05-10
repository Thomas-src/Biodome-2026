using System.Text.RegularExpressions;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Extensions;
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

    public Equipement Equipement { get; set; }

    public IActionResult OnGet(string slug, string ownerName)
    {
        slug = SanitizeInput(slug);
        ownerName = SanitizeInput(ownerName);
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(ownerName))
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
        slug = SanitizeInput(slug).ToSlug();
        ownerName = SanitizeInput(ownerName);
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var biodomesHasSpecies = _biodomesService.HasEquipement(slug, ownerName);
        if (biodomesHasSpecies.Count > 0)
        {
            TempData["ErrorMessage"] = "L'équipement est utilisé dans au moins un biodome, il est donc impossible de le supprimer";
            return RedirectToPage("/Admin/species");
        }
        _service.Delete(slug, ownerName);
        TempData["SuccessMessage"] = "Équipement supprimé avec succès";
        return RedirectToPage("/Admin/Equipements");
      
    }
    private string SanitizeInput(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
    
}