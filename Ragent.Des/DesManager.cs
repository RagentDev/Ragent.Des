using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesManager : IDesManager
{
    private readonly DesService _desService;
    
    public DesManager(Dictionary<Type, Type> mappings, Dictionary<Type, object> existing)
    {
        _desService = new DesService(mappings, existing);
    }

    public T GetService<T>()
    {
        return (T) _desService.GetService(typeof(T));
    }

    public bool RecycleService<T>()
    {
        throw new NotImplementedException();
    }

    public void RegisterEvent<T, TE>(Action<TE> callback)
    {
        _desService.RegisterEvent(typeof(T), typeof(TE), callback.Target, callback.Method);
    }

    public void UnregisterEvent<T, TE>(Action<TE> callback)
    {
        _desService.UnregisterEvent(typeof(T), typeof(TE), callback.Target, callback.Method);
    }
}