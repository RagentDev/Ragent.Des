# Ragent.Des
Dependency Injection, Events, Services

[![](https://dcbadge.vercel.app/api/server/qfU23sXBmy)](https://discord.gg/qfU23sXBmy)

### Use Des Builder to create Des Manager
```C#
public class ExampleForReadme
{
    private IDesManager _manager;

    public void CreateDesBuilder()
    {
        _manager = new DesBuilder()
            .AddService<Service>()
            .Build();
    }
}

public class Service { }
```

Types can also be initialized with any of the following.
```C#
.AddService<Service>()
.AddService<Service>(new Service(args))
.AddService<Interface, Service>()
```

### Fetching Services
```C#
public class ExampleForReadme
{
    private IDesManager _manager;

    public void CreateDesBuilder()
    {
        _manager = new DesBuilder()
            .AddService<Service>()
            .Build();
    }

    public void GetCountFromService()
    {
        var service = _manager.GetService<Service>();
        var count = service.GetCount();
    }
}

public class Service
{
    private int count = 0;

    public int GetCount()
    {
        return count;
    }
}
```
### Registering Events
```C#
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
```
