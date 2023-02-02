using System.Collections.ObjectModel;
using System.Reflection;
using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesServiceManager : IDesServiceManager
{
    // TODO: Separate Managers
    // DesManager - Top level
    // DesServiceManager - inside DesManager
    // DesEventManager - inside DesManager
    private readonly ReadOnlyDictionary<Type, Type> _typeMappings;
    private readonly Dictionary<Type, object> _services;
    private readonly Dictionary<Type, EventInfo[]> _events;

    public DesServiceManager(IDictionary<Type, Type> types)
    {
        _typeMappings = new ReadOnlyDictionary<Type, Type>(types);
        _services = new Dictionary<Type, object>();
        _events = new Dictionary<Type, EventInfo[]>();
        
        foreach (var keyValuePair in _typeMappings.Where(pair => !_services.ContainsKey(pair.Key)))
        {
            InternalCreateServiceFromType(keyValuePair.Key, keyValuePair.Value);
        }
        foreach (var keyValuePair in _typeMappings)
        {
            InternalCreateEventsFromType(keyValuePair.Key, keyValuePair.Value);
        }
    }
    
    public TI GetService<TI>()
    {
        return (TI)_services[typeof(TI)];
    }

    public TI RemoveService<TI>()
    {
        throw new NotImplementedException();
    }

    public TI RecycleService<TI>()
    {
        throw new NotImplementedException();
    }

    public void RegisterEvent<TI, TE>(Action<TE> callback)
    {
        var eventInfo = InternalGetEventInfo(typeof(TI), typeof(TE));

        if (eventInfo.EventHandlerType != null)
        {
            eventInfo.AddEventHandler(_services[typeof(TI)], 
                Delegate.CreateDelegate(eventInfo.EventHandlerType, callback.Target, callback.Method));
        }
        else
        {
            throw new Exception();
        }
    }

    private EventInfo InternalGetEventInfo(Type inter, Type obj)
    {
        return _events[inter].Single(e => e.EventHandlerType.GenericTypeArguments[0] == obj) ?? throw new Exception();
    }
    
    private object InternalResolveMissingService(Type obj)
    {
        if (_typeMappings.TryGetValue(obj, out var inter))
        {
            if (_services.TryGetValue(inter, out var instance))
                return instance;
            
            InternalCreateServiceFromType(inter, obj);
        }

        if (inter != null) return _services[inter];
        throw new Exception();
    }
    
    private void InternalCreateServiceFromType(Type inter, Type obj)
    {
        var constructors = obj.GetConstructors();

        if (constructors.Length == 0)
        {
            var instance = Activator.CreateInstance(obj);
            if (instance != null)
            {
                _services.Add(inter, instance);
            }
            else
            {
                throw new Exception();
            }
        }
        else
        {
            var parameters = constructors[0].GetParameters();
            var resolvedParams = parameters.Select(item => InternalResolveMissingService(item.ParameterType)).ToList();

            var instance = constructors[0].Invoke(resolvedParams.ToArray());
            _services.Add(inter, instance);
        }
    }

    private void InternalCreateEventsFromType(Type inter, Type obj)
    {
        var events = obj.GetEvents();
        _events.Add(inter, events);
    }
}