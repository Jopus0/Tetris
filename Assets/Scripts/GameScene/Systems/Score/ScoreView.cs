using System;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreView : MonoBehaviour, IInitializable, IDisposable
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private Score _score;

    [Inject]
    private void Construct(Score score)
    {
        _score = score;
    }
    public void Initialize()
    {
        _score.OnScoreChanged += UpdateScoreText;
    }
    public void Dispose()
    {
        _score.OnScoreChanged -= UpdateScoreText;
    }
    public void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }
}
