using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class Edit : PageModel
{
    private readonly IRepository _repository;

    private Domains.Core.Species? _espece;

    public Domains.Core.Species? Espece
    {
        get { return _espece;}
        set { _espece = value; }
    }

    public Edit(IRepository repository)
    {
        _repository = repository;
    }


    public void OnGet(string slug)
    {
        Espece = _repository.GetallEspece().FirstOrDefault(e => e.Nom.ToKebabCase()==slug);
    }
}