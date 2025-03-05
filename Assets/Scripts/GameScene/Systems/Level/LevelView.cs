using System;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelView : MonoBehaviour, IInitializable, IDisposable
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private Level _level;

    [Inject]
    private void Construct(Level level)
    {
        _level = level;
    }
    public void Initialize()
    {
        _level.OnLevelChanged += UpdateLevelText;
    }
    public void Dispose()
    {
        _level.OnLevelChanged -= UpdateLevelText;
    }
    public void UpdateLevelText(int level)
    {
        _levelText.text = level.ToString();
    }
}
