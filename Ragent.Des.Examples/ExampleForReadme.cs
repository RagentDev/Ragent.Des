using Ragent.Des.Interfaces;

namespace Ragent.Des.Examples;

public class ExampleForReadme
{
    private IDesManager _manager;

    public void CreateDesBuilder()
    {
        _manager = new DesBuilder()
            .AddService<Service>()
            .Build();
        
        _manager.RegisterEvent<Service, Event>(GetCountFromService);
    }

    private void GetCountFromService(Event ev)
    {
        Console.WriteLine($"{ev.Amount} | {ev.NewCount}");
    }
}

public struct Event
{
    public int Amount { get; set; }
    public int NewCount { get; set; }
}

public class Service
{
    private event Action<Event>? ScoreAdded;
    private int count = 0;

    public void Add(int amount)
    {
        count += amount;
        ScoreAdded?.Invoke(new Event
        {
            Amount = amount,
            NewCount = count
        });
    }
}