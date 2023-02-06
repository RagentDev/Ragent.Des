using System.Reflection;

namespace Ragent.Des;

public class DesService
{
    private readonly Dictionary<Type, object> _services;
    private readonly Dictionary<Type, EventInfo[]> _events;
    private readonly Dictionary<Type, Type> _mappings;

    public DesService(Dictionary<Type, Type> mapping, Dictionary<Type, object> existing)
    {
        _services = existing;
        _events = new Dictionary<Type, EventInfo[]>();
        _mappings = mapping;
        
        foreach (var map in _mappings.Where(pair => !_services.ContainsKey(pair.Key)))
        {
            CreateService(map.Key, map.Value);
        }
        foreach (var map in mapping)
        {
            CreateEvents(map.Key, map.Value);
        }
    }

    public object GetService(Type interfaceType)
    {
        if (_services.TryGetValue(interfaceType, out var obj))
            return obj;

        // Throws DesServiceNotFound Exception
        throw new Exception();
    }
    
    public void RegisterEvent(Type interfaceType, Type eventType, object callbackTarget, MethodInfo callbackMethod)
    {
        var eventInfo = GetEventInfo(interfaceType, eventType);

        if (eventInfo.EventHandlerType != null)
        {
            eventInfo.AddEventHandler(GetService(interfaceType),
                Delegate.CreateDelegate(eventInfo.EventHandlerType, callbackTarget, callbackMethod));
        }
        else
        {
            // Throw DesEventExistence
            throw new Exception();
        }
    }
    
    public void UnregisterEvent(Type interfaceType, Type eventType, object callbackTarget, MethodInfo callbackMethod)
    {
        var eventInfo = GetEventInfo(interfaceType, eventType);

        if (eventInfo.EventHandlerType != null)
        {
            eventInfo.RemoveEventHandler(GetService(interfaceType),
                Delegate.CreateDelegate(eventInfo.EventHandlerType, callbackTarget, callbackMethod));
        }
        else
        {
            // Throw DesEventExistence
            throw new Exception();
        }
    }
    
    private void CreateService(Type interfaceType, Type objectType)
    {
        var constructors = objectType.GetConstructors();

        if (constructors.Length == 0)
        {
            CreateServiceNoInjection(interfaceType, objectType);
        }
        else
        {
            CreateServiceWithConstructor(constructors, interfaceType, objectType);
        }
    }

    private void CreateServiceNoInjection(Type interfaceType, Type objectType)
    {
        try
        {
            var instance = Activator.CreateInstance(objectType);
            _services.Add(interfaceType, instance);
        }
        catch (MissingMethodException)
        {
            // Throw DesConstructorException
            throw new Exception();
        }
    }

    private void CreateServiceWithConstructor(ConstructorInfo[] constructorInfos, Type interfaceType, Type objectType)
    {
        var created = false;
        
        foreach (var constructor in constructorInfos)
        {
            // Make sure we can actually invoke this constructor
            var canCreate = true;
            var parameters = constructor.GetParameters();

            foreach (var parameter in parameters)
            {
                if (!_mappings.ContainsKey(parameter.ParameterType))
                    canCreate = false;
            }

            if (canCreate)
            {
                var resolvedParams = parameters.Select(item => InternalResolveMissingService(item.ParameterType)).ToList();
                var instance = constructor.Invoke(resolvedParams.ToArray());
                _services.Add(interfaceType, instance);
                created = true;
            }
        }
        
        // If we couldn't invoke using the constructor, default back to no injection
        if (!created)
            CreateServiceNoInjection(interfaceType, objectType);
    }
    
    private object InternalResolveMissingService(Type obj)
    {
        if (_mappings.TryGetValue(obj, out var inter))
        {
            if (_services.TryGetValue(inter, out var instance))
                return instance;
            
            CreateService(inter, obj);
        }
        
        // Throw DesServiceNotFound
        throw new Exception();
    }
    
    private EventInfo GetEventInfo(Type inter, Type obj)
    {
        return _events[inter].Single(e => e.EventHandlerType.GenericTypeArguments[0] == obj) ?? throw new Exception();
    }

    private void CreateEvents(Type inter, Type obj)
    {
        var events = obj.GetEvents();
        _events.Add(inter, events);
    }
    
}