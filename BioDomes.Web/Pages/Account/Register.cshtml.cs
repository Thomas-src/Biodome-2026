using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;

public class Register : PageModel
{
   
    private readonly SignInManager<Infrastructures.entities.User> _signInManager;
    private readonly UserManager<Infrastructures.entities.User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserStore<Infrastructures.entities.User> _userStore;
    private readonly IUserEmailStore<Infrastructures.entities.User> _emailStore;
    private readonly ILogger<Register> _logger;
    private readonly IEmailSender _emailSender;

    public Register(
        UserManager<Infrastructures.entities.User> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserStore<Infrastructures.entities.User> userStore,
        SignInManager<Infrastructures.entities.User> signInManager,
        ILogger<Register> logger,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    
    public IList<string> Organismes
    {
        get => _organismes;
        set => _organismes = value;
    }

    private IList<string> _organismes = new List<string>();
    public void OnGet()
    {
    }

    
    [BindProperty]
    public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }
    
    
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress] // todo faire un bon filtre
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
            
        [Display(Name = "ResearchOrganism")] public string ResearchOrganism { get; set; }
            
        [Display(Name = "Level")] 
        [Range(1,10)]
        public int Level { get; set; }
    }

    
    private IUserEmailStore<Infrastructures.entities.User> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<Infrastructures.entities.User>)_userStore;
    }

}