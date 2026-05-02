namespace BioDomes.Domains.Repository.Exceptions;

public class ReadBiodomesException : Exception
{
    public  ReadBiodomesException(string message, Exception? cause = null) : base(message, cause)
    {
    }
}