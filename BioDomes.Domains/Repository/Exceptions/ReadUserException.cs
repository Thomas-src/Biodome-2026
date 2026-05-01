namespace BioDomes.Domains.Repository.Exceptions;

public class ReadUserException : Exception
{
    public ReadUserException(string message, Exception? cause = null) : base(message, cause)
    {
        
    }
}