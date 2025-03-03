using UnityEngine;
using Zenject;
public class FigureGenerator
{
    private FigureGeneratorSettings _settings;
    public FigureGenerator(FigureGeneratorSettings settings)
    {
        _settings = settings;
    }
    public FigureSettings GetRandomFigure()
    {
        int randomNumber = Random.Range(0, _settings.FiguresSettings.Length);
        return _settings.FiguresSettings[randomNumber];
    }
}
