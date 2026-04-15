using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class SpeciesModel : PageModel
{
    private readonly ISpeciesRepository _repository;

    public IReadOnlyList<Domains.Core.Species> _especes;

    public IReadOnlyList<Domains.Core.Species> Especes
    {
        get { return _especes; }
        set { _especes = value; }
    }

    public SpeciesModel(ISpeciesRepository repository)
    {
        _repository = repository;
    }

public void OnGet()
{
    Especes = _repository.GetAllSpecies();
}
}