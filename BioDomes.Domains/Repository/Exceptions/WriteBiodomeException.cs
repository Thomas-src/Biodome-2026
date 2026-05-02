namespace BioDomes.Domains.Repository.Exceptions;

public class WriteBiodomeException : Exception
{
    public WriteBiodomeException(string message, Exception? cause = null) : base(message, cause)
    {
        
    }
}