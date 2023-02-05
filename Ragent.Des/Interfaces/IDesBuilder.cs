namespace Ragent.Des.Interfaces;

public interface IDesBuilder
{
    public DesBuilder AddService<T>();

    public DesBuilder AddService<T>(T obj);

    public DesBuilder AddService<TI, T>() where T : TI;

    public DesManager Build();
}