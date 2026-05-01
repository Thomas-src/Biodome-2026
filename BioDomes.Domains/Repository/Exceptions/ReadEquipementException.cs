namespace BioDomes.Domains.Repository.Exceptions;

public class ReadEquipementException : Exception
{
    public ReadEquipementException(string message, Exception? cause) : base(message, cause) { }
}