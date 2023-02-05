using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesManager : IDesManager
{

    private readonly IDesServiceManager _serviceManager;
    private readonly IDesEventManager _eventManager;

    private readonly Dictionary<Type, Type> _typeMappings;
    
    public DesManager(Dictionary<Type, Type> mappings, Dictionary<Type, object> existing)
    {
        _typeMappings = mappings;
        _serviceManager = new DesServiceManager(this, _typeMappings, existing);
        _eventManager = new DesEventManager(this, _typeMappings);
    }

    internal bool GetServiceMapping(Type type, out Type mapping)
    {
        if (_typeMappings.ContainsKey(type))
        {
            mapping = _typeMappings[type];
            return true;
        }

        mapping = null;
        return false;
    }

    internal object GetRawService(Type inter)
    {
        return _serviceManager.GetService(inter);
    }

    public T GetService<T>()
    {
        return (T) _serviceManager.GetService(typeof(T));
    }

    public T RemoveService<T>()
    {
        throw new NotImplementedException();
    }

    public T RecycleService<T>()
    {
        throw new NotImplementedException();
    }

    public void RegisterEvent<T, TE>(Action<TE> callback)
    {
        _eventManager.RegisterEvent(typeof(T), typeof(TE), callback.Target, callback.Method);
    }

    public void UnregisterEvent<T, TE>(Action<TE> callback)
    {
        _eventManager.UnregisterEvent(typeof(T), typeof(TE), callback.Target, callback.Method);
    }
}