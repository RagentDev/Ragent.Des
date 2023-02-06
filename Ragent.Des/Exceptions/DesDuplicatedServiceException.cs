namespace Ragent.Des.Exceptions;

public class DesDuplicatedServiceException : Exception
{
    public DesDuplicatedServiceException(Type type) : base(type.ToString())
    {
        
    }
}