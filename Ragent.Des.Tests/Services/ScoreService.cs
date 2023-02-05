using Ragent.Des.Tests.Services.Events;
using Ragent.Des.Tests.Services.Interfaces;

namespace Ragent.Des.Tests.Services;

public class ScoreService : IScoreService
{
    public event Action<EventScoreAdded>? ScoreAdded;

    private int _score = 0;

    public int GetScore()
    {
        return _score;
    }
    
    public void AddScore(int amount)
    {
        _score += amount;
        
        ScoreAdded?.Invoke(new EventScoreAdded
        {
            amount = amount,
            newScore = _score
        });
    }
}