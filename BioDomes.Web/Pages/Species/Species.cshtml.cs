using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class SpeciesModel : PageModel
{
    public IList<Domains.Core.Species> _especes;

    public IList<Domains.Core.Species> Especes
    {
        get { return _especes; }
        set { _especes = value; }
    }

    public void OnGet()
    {
       // IRepository repository = new SqlRepository();
       // _especes = repository.GetallEspece();
    }
}