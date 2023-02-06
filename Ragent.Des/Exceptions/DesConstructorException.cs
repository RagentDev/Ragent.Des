namespace Ragent.Des.Exceptions;

public class DesConstructorException : Exception
{
    public DesConstructorException(Type type) : base(type.ToString())
    {
        
    }
}