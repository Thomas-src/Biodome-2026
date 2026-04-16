using BioDomes.Domains.Constants;

namespace BioDomes.Domains.Core;

public enum Roles
{
    Admin,
    Research
}
public static class RoleHelper
{
    public static string ToRoleName(this Roles role)
    {
        return role switch
        {
            Roles.Admin => AppRoles.Admin,
            Roles.Research =>  AppRoles.Research,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}