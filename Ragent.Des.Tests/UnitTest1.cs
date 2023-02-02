using System.Diagnostics;
using Ragent.Des.Tests.Services;
using Ragent.Des.Tests.Services.Events;
using Ragent.Des.Tests.Services.Interfaces;

namespace Ragent.Des.Tests;

public class Tests
{

    private DesServiceManager _serviceManager;
    
    [SetUp]
    public void Setup()
    {
        _serviceManager = new DesServiceBuilder()
            .AddService<IScoreService, ScoreService>()
            .Build();
        
        _serviceManager.RegisterEvent<IScoreService, EventScoreAdded>(ScoreChanged);
    }

    private void ScoreChanged(EventScoreAdded scoreAdded)
    {
        Console.WriteLine($"{scoreAdded.newScore} was changed by {scoreAdded.amount}");
    }

    [Test]
    public void Test1()
    {
        var scoreService = _serviceManager.GetService<IScoreService>();

        var stopWatch = new Stopwatch();
        
        stopWatch.Start();
        scoreService.AddScore(20);
        scoreService.AddScore(10);
        scoreService.AddScore(5);
        scoreService.AddScore(2);
        scoreService.AddScore(1);
        stopWatch.Stop();
        
        Assert.Pass();
    }
}