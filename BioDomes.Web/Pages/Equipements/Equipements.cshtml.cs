using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Equipements;

public class Equipements : PageModel
{
    private readonly IEquipementService _service;
    
    private IReadOnlyList<Equipement> _restEquipements;

    public IReadOnlyList<Equipement> RestEquipements
    {
        get { return _restEquipements; }
        set { _restEquipements = value; }
    }
    
    private IReadOnlyList<Equipement> _ownEquipements = new List<Equipement>();

    public IReadOnlyList<Equipement> OwnEquipements
    {
        get { return _ownEquipements; }
        set { _ownEquipements = value; }
    }

    public Equipements(IEquipementService service)
    {
        _service = service;
    }
    
    public void OnGet()
    {
        IReadOnlyList<Equipement> all = _service.GetAllEquipements();
        
        if (User.Identity.IsAuthenticated)
        {
            OwnEquipements = _service.FilterOwn(all,  User.Identity.Name);
            RestEquipements = all.Where(x => !OwnEquipements.Contains(x)).ToList();
        }
        else
        {
            RestEquipements = all;
        }
        
    }
}