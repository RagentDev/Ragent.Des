using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesServiceManager : IDesServiceManager
{
    private readonly DesManager _desManager;
    private readonly Dictionary<Type, object> _services;

    public DesServiceManager(DesManager desManager, Dictionary<Type, Type> mapping, Dictionary<Type, object> existing)
    {
        _desManager = desManager;
        _services = new Dictionary<Type, object>();

        foreach (var map in mapping.Where(pair => !_services.ContainsKey(pair.Key)))
        {
            CreateService(map.Key, map.Value);
        }
        
        foreach (var keyPair in existing)
        {
            _services.Add(keyPair.Key, keyPair.Value);
        }
    }

    public object GetService(Type inter)
    {
        return _services[inter];
    }

    private void CreateService(Type inter, Type obj)
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
    
    private object InternalResolveMissingService(Type obj)
    {
        if (_desManager.GetServiceMapping(obj, out var inter))
        {
            if (_services.TryGetValue(inter, out var instance))
                return instance;
            
            CreateService(inter, obj);
        }
        
        throw new Exception();
    }
}