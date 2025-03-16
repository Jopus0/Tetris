using System;

public class Score 
{
    public event Action<int> OnScoreChanged;
    private int _score;
    public Score()
    {
        _score = 0;
    }
    public int GetScore()
    {
        return _score;
    }
    public void AddScore(int score)
    {
        _score += score;
        OnScoreChanged?.Invoke(_score);
    }
}
