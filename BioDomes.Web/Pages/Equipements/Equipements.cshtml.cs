using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Equipements;

public class Equipements : PageModel
{
    private readonly IEquipementService _service;

    public IReadOnlyList<Equipement> RestEquipements { get; set; }

    public IReadOnlyList<Equipement> OwnEquipements { get; set; } = new List<Equipement>();

    public Equipements(IEquipementService service)
    {
        _service = service;
    }
    
    public void OnGet()
    {
        try
        {
            IReadOnlyList<Equipement> all = _service.GetAllEquipements();
        
            if (User.Identity.IsAuthenticated)
            {
                OwnEquipements = _service.FilterOwn(all,  User.Identity.Name);
                RestEquipements = all.Where(x => !OwnEquipements.Contains(x) && x.Visibility).ToList();
            }
            else
            {
                RestEquipements = all.Where(x => x.Visibility).ToList();
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
        }
    }
}