namespace Ragent.Des.Interfaces;

public interface IDesServiceManager
{
    public TI GetService<TI>();

    public TI RemoveService<TI>();

    public TI RecycleService<TI>();

    public void RegisterEvent<TI, TE>(Action<TE> callback);
}