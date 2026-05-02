using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BioDomes.Domains.Extensions;

public static class StringExtensions
{
    public static string ToKebabCase(this string s)
    {
        return s.Trim().ToLower().Replace(" ", "-");
    }

    /**
     * Code de Joan Caron : https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
     * Consulté le 02/05/2026
     */
    public static string ToSlug(this string value){

        //First to lower case
        value = value.ToLowerInvariant();

        //Remove all accents
        var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
        value = Encoding.ASCII.GetString(bytes);

        //Replace spaces
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

        //Remove invalid chars
        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "",RegexOptions.Compiled);

        //Trim dashes from end
        value = value.Trim('-', '_');

        //Replace double occurences of - or _
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

        return value ;
    }
}