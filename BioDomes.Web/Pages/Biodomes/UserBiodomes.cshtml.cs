using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
public class UserBiodomesModel : PageModel
{
    private readonly IBiodomeService _service;
    private readonly UserManager<UserEntity>  _userManager;
    private readonly ILogger<UserBiodomesModel> _logger;
    public UserBiodomesModel(IBiodomeService service, UserManager<UserEntity>  userManager,  ILogger<UserBiodomesModel> logger)
    {
        _service = service;
        _userManager = userManager;
        _logger = logger;
    }

    public IReadOnlyList<Biodome> Allbiodomes;

    public IReadOnlyList<Biodome> AllBiodomes
    {
        get => Allbiodomes;
        set => Allbiodomes = value;
    }

    public void OnGet()
    {
        var user = _userManager.GetUserName(User);
        _logger.LogInformation($"Recherche des biodomes de: {user}");
        AllBiodomes = _service.GetUserBiodomes(user!);
    }
}