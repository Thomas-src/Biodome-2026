namespace BioDomes.Domains.Repository.Exceptions;

public class DeleteBiodomeException : Exception
{
    public DeleteBiodomeException(string message, Exception? cause = null): base(message, cause)
    {
        
    }
}