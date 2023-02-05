using System.Diagnostics;
using Ragent.Des.Interfaces;
using Ragent.Des.Tests.Services;
using Ragent.Des.Tests.Services.Events;
using Ragent.Des.Tests.Services.Interfaces;

namespace Ragent.Des.Tests;

public class Tests
{

    private IDesManager _manager;
    
    [SetUp]
    public void Setup()
    {
        _manager = new DesBuilder()
            .AddService<IScoreService, ScoreService>()
            .Build();
        
        _manager.RegisterEvent<IScoreService, EventScoreAdded>(ScoreChanged);
    }

    private void ScoreChanged(EventScoreAdded scoreAdded)
    {
        Console.WriteLine($"{scoreAdded.newScore} was changed by {scoreAdded.amount}");
    }

    [Test]
    public void Test1()
    {
        var scoreService = _manager.GetService<IScoreService>();

        var stopWatch = new Stopwatch();
        
        stopWatch.Start();
        scoreService.AddScore(20);
        scoreService.AddScore(10);
        scoreService.AddScore(5);
        scoreService.AddScore(2);
        scoreService.AddScore(1);
        stopWatch.Stop();
        
        Assert.That(scoreService.GetScore(), Is.EqualTo(38));
    }
}