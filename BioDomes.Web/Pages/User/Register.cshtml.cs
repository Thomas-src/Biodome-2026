using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.User;

public class Register : PageModel
{
    public IList<string> Organismes
    {
        get => _organismes;
        set => _organismes = value;
    }

    private IList<string> _organismes = new List<string>();
    public void OnGet()
    {
        setOrganismes();
    }

    private void setOrganismes()
    {
        _organismes.Add("ULiege");
        _organismes.Add("UCLouvain");
        _organismes.Add("HELMo");
    }
}