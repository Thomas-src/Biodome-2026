using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

public class Biodomes : PageModel
{
    public IList<Biodome> _allbiodomes;

    public IList<Biodome> AllBiodomes
    {
        get { return _allbiodomes; }
        set { _allbiodomes = value; }
    }

    public void OnGet()
    {
        /*
        IRepository repository = new SqlRepository();

        AllBiodomes = repository.GetAllDomes();
        */
    }
}