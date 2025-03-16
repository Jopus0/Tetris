using System;

public class Level
{
    public event Action<int> OnLevelChanged;
    private int _level;
    public Level()
    {
        _level = 0;
    }
    public int GetLevel()
    {
        return _level;
    }
    public void AddLevel(int level)
    {
        _level += level;
        OnLevelChanged?.Invoke(_level);
    }
}
