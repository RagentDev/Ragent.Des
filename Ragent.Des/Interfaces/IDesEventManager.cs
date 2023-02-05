using System.Reflection;

namespace Ragent.Des.Interfaces;

public interface IDesEventManager
{
    public void RegisterEvent(Type inter, Type ev, object callbackTarget, MethodInfo callbackMethod);

    public void UnregisterEvent(Type inter, Type ev, object callbackTarget, MethodInfo callbackMethod);
}