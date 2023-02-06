namespace Ragent.Des.Exceptions;

public class DesEventMissingException : Exception
{
    public DesEventMissingException(Type type) : base(type.ToString())
    {
        
    }
}