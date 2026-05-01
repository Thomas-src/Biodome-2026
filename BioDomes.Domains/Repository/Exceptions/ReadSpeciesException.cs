namespace BioDomes.Domains.Repository.Exceptions;

public class ReadSpeciesException : Exception
{
    public  ReadSpeciesException(string message, Exception? cause = null) : base(message, cause)
    {
    }
}