namespace BioDomes.Domains.Repository.Exceptions;

public class WriteSpeciesException : Exception
{
    public  WriteSpeciesException(string message, Exception? cause = null) : base(message, cause)
    {
    }
}