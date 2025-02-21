using UnityEngine;
using Zenject;
public class FigureGenerator
{
    private FigureSettings[] _figureSettings;
    public FigureGenerator(FigureSettings[] figureSettings)
    {
        _figureSettings = figureSettings;
    }
    public FigureSettings GetRandomFigure()
    {
        int randomNumber = Random.Range(0, _figureSettings.Length);
        return _figureSettings[randomNumber];
    }
}
