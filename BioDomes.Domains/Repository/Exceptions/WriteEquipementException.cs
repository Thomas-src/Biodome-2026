namespace BioDomes.Domains.Repository.Exceptions;

public class WriteEquipementException : Exception
{
    public  WriteEquipementException(string message, Exception? cause) : base(message, cause) { }
}