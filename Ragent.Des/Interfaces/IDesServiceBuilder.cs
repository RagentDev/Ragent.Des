namespace Ragent.Des.Interfaces;

public interface IDesServiceBuilder
{
    public DesServiceBuilder AddService<TI, TC>() where TC : TI;

    public DesServiceManager Build();
}