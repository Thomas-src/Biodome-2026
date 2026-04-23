using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BioDomes.Web.Validators;


public class IsEmailValid() : ValidationAttribute("L'adresse e-mail n'est pas valide.")
{
    /*
     * Source - https://stackoverflow.com/a/16168103µ
     * Posted by Parimal Raj, modified by community. See post 'Timeline' for change historyµ
     * Retrieved 2026-04-23, License - CC BY-SA 3.0
     */
    private static readonly Regex _regex = new(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }
        return _regex.IsMatch(value.ToString());
    }
}