using System.Collections;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

public class Details : PageModel
{
    
    private readonly IBiodomeService _service;
    private readonly IEquipementRepository _equipementRepository;
    private readonly ISpeciesRepository _speciesRepository;

    public Biodome Biodome { get; set; }
    
    public bool Owner {get; set;} = false;
    public IReadOnlyList<Domains.Core.Species> AvailableSpecies { get; set; }
    public IReadOnlyList<Domains.Core.Equipements.Equipement> AvailableEquipements { get; set; }

    public Details(IBiodomeService service,  ISpeciesRepository speciesRepository, IEquipementRepository equipementRepository)
    {
        _service = service;
        _speciesRepository = speciesRepository;
        _equipementRepository = equipementRepository;
    }
    
    public IActionResult OnGet(int id)
    {
        Biodome = _service.GetBiodomeById(id);

        if (Biodome == null)
        {
            return NotFound();    
        }

        if (User.Identity.IsAuthenticated)
        {
            if (Biodome.CreatorName == User.Identity.Name)
            {
                Owner = true;
                AvailableEquipements = _equipementRepository.GetAllEquipements().Where(x => x.Visibility).ToList();
                AvailableSpecies = _speciesRepository.GetAllSpecies().Where(x => x.IsVisible).ToList();
            }
        }
        
        
        return Page();
    }

    public IActionResult OnPostAddSpecies(int biomeId, string speciesName, int quantity = 1)
    {
        _service.AddSpecyTo(biomeId, speciesName, quantity);
    
        return RedirectToPage(new { id = biomeId });
    }
    
    public IActionResult OnPostAddEquipement(int biomeId, string equipementName)
    {
        _service.AddEquipementTo(biomeId, equipementName);
        
        return RedirectToPage(new { id = biomeId });
    }
    
    public IActionResult OnPostRemoveSpecies(int biomeId, string speciesName)
    {
        _service.RemoveSpecyFrom(biomeId, speciesName);
        // TempData["SuccessMessage"] = $"L'espèce {speciesName} a été retirée du biodome.";
        return RedirectToPage(new { id = biomeId });
    }

    public IActionResult OnPostRemoveEquipement(int biomeId, string equipementName)
    {
        _service.RemoveEquipementFrom(biomeId, equipementName);
        // TempData["SuccessMessage"] = $"L'équipement {equipementName} a été retiré du biodome.";
        return RedirectToPage(new { id = biomeId });
    }
}