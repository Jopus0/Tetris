using Zenject;
using System.Collections.Generic;
using System;
public class ScoreHandler : IInitializable, IDisposable
{
    private ScoreHandlerSettings _settings;
    private Score _score;
    private Field _field;

    private Dictionary<int, int> _linesScore;
    public ScoreHandler(ScoreHandlerSettings settings, Score score, Field field)
    {
        _settings = settings;
        _score = score;
        _field = field;
        _linesScore = new Dictionary<int, int>();
    }
    public void Initialize()
    {
        _field.OnLinesCleared += AddLinesScore;

        foreach(var lineScore in _settings.LinesScore)
            _linesScore.Add(lineScore.Count, lineScore.Score);
    }
    public void Dispose()
    {
        _field.OnLinesCleared -= AddLinesScore;
    }
    public void AddLinesScore(int count)
    {
        int score = 0;
        _linesScore.TryGetValue(count, out score);
        _score.AddScore(score);
    }
}
