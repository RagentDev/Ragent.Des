namespace Ragent.Des.Interfaces;

public interface IDesManager
{
    public T GetService<T>();

    public T RemoveService<T>();

    public T RecycleService<T>();

    public void RegisterEvent<T, TE>(Action<TE> callback);

    public void UnregisterEvent<T, TE>(Action<TE> callback);
}