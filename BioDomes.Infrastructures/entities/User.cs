using Microsoft.AspNetCore.Identity;
namespace BioDomes.Infrastructures.entities;

public class User : IdentityUser
{
    public string Avatar { get; set; } = string.Empty;
}