namespace Ragent.Des.Exceptions;

public class DesServiceMissingException : Exception
{
    public DesServiceMissingException(Type type) : base(type.ToString())
    {
        
    }
}