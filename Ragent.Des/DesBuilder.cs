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
        // Throw DesServiceExists exception
        if (_interfaceMapping.ContainsKey(typeof(T)) || _interfaceMapping.ContainsValue(typeof(T)))
            throw new Exception();
        
        _interfaceMapping.Add(typeof(T), typeof(T));
        return this;
    }

    public DesBuilder AddService<T>(T obj)
    {
        // Throw DesServiceExists exception
        if (_interfaceMapping.ContainsKey(typeof(T)) || _interfaceMapping.ContainsValue(typeof(T)))
            throw new Exception();
        
        _objectMapping.Add(typeof(T), obj);
        _interfaceMapping.Add(typeof(T), typeof(T));
        return this;
    }

    public DesBuilder AddService<TI, T>() where T : TI
    {
        // Throw DesServiceExists exception
        if (_interfaceMapping.ContainsKey(typeof(T)) || _interfaceMapping.ContainsValue(typeof(T)) || _interfaceMapping.ContainsKey(typeof(TI)))
            throw new Exception();
        
        _interfaceMapping.Add(typeof(TI), typeof(T));
        return this;
    }
    
    public DesBuilder AddService<TI, T>(T obj) where T : TI
    {
        // Throw DesServiceExists exception
        if (_interfaceMapping.ContainsKey(typeof(T)) || _interfaceMapping.ContainsValue(typeof(T)) || _interfaceMapping.ContainsKey(typeof(TI)))
            throw new Exception();
        
        _objectMapping.Add(typeof(TI), obj);
        _interfaceMapping.Add(typeof(TI), typeof(T));
        return this;
    }

    public DesManager Build()
    {
        return new DesManager(_interfaceMapping, _objectMapping);
    }
}