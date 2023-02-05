using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesBuilder : IDesBuilder
{
    private readonly Dictionary<Type, Type> _interfaceMapping;
    private readonly Dictionary<Type, object> _objectMapping;

    public DesBuilder()
    {
        _interfaceMapping = new Dictionary<Type, Type>();
        _objectMapping = new Dictionary<Type, object>();
    }

    public DesBuilder AddService<T>()
    {
        _interfaceMapping.Add(typeof(T), typeof(T));
        return this;
    }

    public DesBuilder AddService<T>(T obj)
    {
        _objectMapping.Add(typeof(T), obj);
        return this;
    }

    public DesBuilder AddService<TI, T>() where T : TI
    {
        _interfaceMapping.Add(typeof(TI), typeof(T));
        return this;
    }

    public DesManager Build()
    {
        return new DesManager(_interfaceMapping, _objectMapping);
    }
}