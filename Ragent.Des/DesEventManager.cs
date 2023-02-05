using System.Reflection;
using Ragent.Des.Interfaces;

namespace Ragent.Des;

public class DesEventManager : IDesEventManager
{
    private readonly DesManager _desManager;
    private readonly Dictionary<Type, EventInfo[]> _events;

    public DesEventManager(DesManager desManager, Dictionary<Type, Type> mapping)
    {
        _desManager = desManager;
        _events = new Dictionary<Type, EventInfo[]>();
        
        foreach (var map in mapping)
        {
            CreateEvents(map.Key, map.Value);
        }
    }

    public void RegisterEvent(Type inter, Type ev, object callbackTarget, MethodInfo callbackMethod)
    {
        var eventInfo = GetEventInfo(inter, ev);

        if (eventInfo.EventHandlerType != null)
        {
            eventInfo.AddEventHandler(_desManager.GetRawService(inter),
                Delegate.CreateDelegate(eventInfo.EventHandlerType, callbackTarget, callbackMethod));
        }
        else
        {
            throw new Exception();
        }
    }
    
    public void UnregisterEvent(Type inter, Type ev, object callbackTarget, MethodInfo callbackMethod)
    {
        var eventInfo = GetEventInfo(inter, ev);

        if (eventInfo.EventHandlerType != null)
        {
            eventInfo.RemoveEventHandler(_desManager.GetRawService(inter),
                Delegate.CreateDelegate(eventInfo.EventHandlerType, callbackTarget, callbackMethod));
        }
        else
        {
            throw new Exception();
        }
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