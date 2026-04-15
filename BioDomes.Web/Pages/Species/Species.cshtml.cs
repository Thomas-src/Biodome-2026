using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

/*
 * TODO, Faire une liste d'espece que le user as (diviser en deux) et une liste des autres especes
 */
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