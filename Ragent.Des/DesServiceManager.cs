using System.Collections.ObjectModel;
using System.Reflection;
using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesServiceManager : IDesServiceManager
{

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
            CreateServiceFromType(keyValuePair.Key, keyValuePair.Value);
        }
    }
    
    public TI GetService<TI>()
    {
        throw new NotImplementedException();
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
        var eventInfo = GetInternalEventInfo<TI, TE>();

        if (eventInfo.EventHandlerType != null)
        {
            eventInfo.AddEventHandler(GetInternalService<TI>(), 
                Delegate.CreateDelegate(eventInfo.EventHandlerType, callback.Target, callback.Method));
        }
        else
        {
            throw new Exception();
        }
    }

    private void CreateInternalEvents()
    {
        
    }

    private TI GetInternalService<TI>()
    {
        if (_services.TryGetValue(typeof(TI), out var service))
            return (TI) service;
        
        throw new Exception();
    }

    private EventInfo GetInternalEventInfo<TI, TE>()
    {
        return _events[typeof(TI)].
            FirstOrDefault(e => e.EventHandlerType == typeof(TE)) ?? throw new Exception();
    }
    
    private object ResolveMissingService(Type obj)
    {
        if (_typeMappings.TryGetValue(obj, out var inter))
        {
            if (_services.TryGetValue(inter, out var instance))
                return instance;
            
            CreateServiceFromType(inter, obj);
        }

        if (inter != null) return _services[inter];
        throw new Exception();
    }
    
    private void CreateServiceFromType(Type inter, Type obj)
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
            var resolvedParams = parameters.Select(item => ResolveMissingService(item.ParameterType)).ToList();

            var instance = constructors[0].Invoke(resolvedParams.ToArray());
            _services.Add(inter, instance);
        }
    }
}