using Ragent.Des.Tests.Builder.Interfaces;

namespace Ragent.Des.Tests.Builder.Services;

public class BasicService : IBasicService
{
    private readonly int _score = 0;

    public BasicService() {}

    public BasicService(int score)
    {
        _score = score;
    }

    public int GetScore()
    {
        return _score;
    }
}