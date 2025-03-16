using System;
using UnityEngine;
using Zenject;
public class FieldHandler : IInitializable, IDisposable, ITickable
{
    private Field _field;
    private FigureMover _figureMover;
    private FigureGenerator _figureGenerator;

    private MatrixPosition _spawnPosition = new MatrixPosition(1, 5);
    private bool _isActive;
    public FieldHandler(Field field, FigureMover figureMover, FigureGenerator figureGenerator)
    {
        _field = field;
        _figureMover = figureMover;
        _figureGenerator = figureGenerator;

        _isActive = true;
    }
    public void Initialize()
    {
        _field.OnBlockOutOffZone += StopCreating;
        _field.OnBlocksLocked += CreateFigure;
        CreateFigure();
    }
    public void Dispose()
    {
        _field.OnBlockOutOffZone -= StopCreating;
        _field.OnBlocksLocked -= CreateFigure;
    }
    public void Tick()
    {
        if (!_isActive)
            return;

        _field.ClearLines();
    }
    public void CreateFigure()
    {
        if (!_isActive)
            return;

        FigureSettings figureSettings = _figureGenerator.GetNextFigure();
        MatrixPosition[] figure = figureSettings.GetFigureCopy();

        int count = figure.Length;
        for (int i = 0; i < count; i++)
        {
            figure[i].Row += _spawnPosition.Row;
            figure[i].Column += _spawnPosition.Column;
        }
        _field.CreateBlocks(figureSettings, figure);
        _figureMover.SetFigure(figure);
    }
    private void StopCreating()
    {
        _isActive = false;
        _figureMover.SetFigure(null);
    }
}
