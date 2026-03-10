namespace BioDomes.Domains.Extensions;

public static class StringExtensions
{
    public static string ToKebabCase(this string s)
    {
        return s.Trim().ToLower().Replace(" ", "-");
    }
}