using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesServiceBuilder : IDesServiceBuilder
{
    private readonly Dictionary<Type, Type> _types;

    public DesServiceBuilder()
    {
        _types = new Dictionary<Type, Type>();
    }

    public DesServiceBuilder AddService<TI, TC>() where TC : TI
    {
        _types.Add(typeof(TI), typeof(TC));
        return this;
    }

    public DesServiceManager Build()
    {
        return new DesServiceManager(_types);
    }
}