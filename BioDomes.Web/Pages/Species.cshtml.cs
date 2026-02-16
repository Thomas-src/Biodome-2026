using BioDomes.Domains;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class SpeciesModel : PageModel
{
    public IList<Espece> especes;

    public void OnGet()
    {
        IRepository repository = new SqlRepository();
        especes = repository.getallEspece();
    }
}