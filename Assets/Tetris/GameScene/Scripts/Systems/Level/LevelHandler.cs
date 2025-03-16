using Zenject;
using System.Collections.Generic;
using System;
public class LevelHandler : IInitializable, IDisposable
{
    private LevelHandlerSettings _settings;
    private Level _level;
    private Field _field;
    private FigureMover _figureMover;

    private Dictionary<int, float> _levelComplication;
    private int _linesForLevel;
    private float _lastLevelFallTime;

    private int _currentLinesCount;
    public LevelHandler(LevelHandlerSettings settings, Level level, Field field, FigureMover figureMover)
    {
        _settings = settings;
        _level = level;
        _field = field;
        _figureMover = figureMover;

        _levelComplication = new Dictionary<int, float>();
        _linesForLevel = _settings.LinesForLevel;
        _currentLinesCount = 0;
    }
    public void Initialize()
    {
        _field.OnLinesCleared += AddLinesCount;

        foreach (var levelComplication in _settings.LevelComplications)
            _levelComplication.Add(levelComplication.Level, levelComplication.FallTime);
    }
    public void Dispose()
    {
        _field.OnLinesCleared -= AddLinesCount;
    }
    public void AddLinesCount(int count)
    {
        int linesCount = _currentLinesCount + count;
        int levelPassed = linesCount / _linesForLevel;
        if (levelPassed > 0)
        {
            _level.AddLevel(levelPassed);

            float fallTime = _lastLevelFallTime;
            _levelComplication.TryGetValue(_level.GetLevel(), out fallTime);
            _figureMover.SetBaseFallTime(fallTime);

            _lastLevelFallTime = fallTime;
            _currentLinesCount = linesCount % _linesForLevel;
        }
        else
        {
            _currentLinesCount = linesCount;
        }
    }
}
