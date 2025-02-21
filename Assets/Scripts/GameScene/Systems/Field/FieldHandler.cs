using System;
using UnityEngine;
using Zenject;
public class FieldHandler : IInitializable, IDisposable, ITickable
{
    private FieldSettings _fieldSettings;
    private FieldView _fieldView;
    private FigureGenerator _figureGenerator;
    private IInput _input;

    private bool[,] _field;
    private MatrixPosition[] _currentFigure;

    private float baseFallTime = 2f;
    private float fallTime = 2f;
    private float fallTimer;

    private float moveTime = 0.5f;
    private float moveTimer;
    public FieldHandler(FieldSettings fieldSettings, FieldView fieldView, FigureGenerator figureGenerator, IInput input)
    {
        _fieldSettings = fieldSettings;
        _fieldView = fieldView;
        _figureGenerator = figureGenerator;
        _input = input;

        fallTimer = 0f;
    }
    public void Initialize()
    {
        _input.OnFall += FigureFallAcceleration; 
        _input.OnMove += FigureMove; 

        _field = new bool[_fieldSettings.Height, _fieldSettings.Width];
        _fieldView.GenerateGrid(_fieldSettings.Height, _fieldSettings.Width, _fieldSettings.StartHeight);
        SpawnFigure();
    }
    public void Dispose()
    {
        _input.OnFall -= FigureFallAcceleration;
        _input.OnMove -= FigureMove;
    }
    public void Tick()
    {
        if (_currentFigure == null)
            return;

        FigureFall();
    }
    private void FigureFall()
    {
        if(fallTimer >= fallTime)
        {
            FigureLinearMove(_currentFigure, 1, 0);
            fallTimer = 0f;
        }
        else
        {
            fallTimer += Time.deltaTime;
        }
    }
    private void FigureFallAcceleration(float axis)
    {
        fallTime = fallTime * (axis * 0.5f);
    }
    private void FigureMove(float axis)
    {
        if (moveTimer >= moveTime)
        {
            int deltaRows = axis > 0 ? 1 : -1;
            FigureLinearMove(_currentFigure, 0, deltaRows);
            moveTimer = 0f;
        }
        else
        {
            moveTimer += Time.deltaTime;
        }
    }
    private void FigureLinearMove(MatrixPosition[] figure, int deltaColumns, int deltaRows)
    {
        int count = figure.Length;
        MatrixPosition[] prevPositions = new MatrixPosition[count];
        for (int i = 0; i < count; i++)
        {
            prevPositions[i] = figure[i];
            figure[i].Column += deltaColumns;
            figure[i].Row += deltaRows;
        }
        _fieldView.MoveBlocks(prevPositions, figure);
    }
    public void SpawnFigure()
    {
        FigureSettings figureSettings = _figureGenerator.GetRandomFigure();
        _currentFigure = figureSettings.GetFigureCopy();
        _fieldView.CreateFigure(figureSettings, _fieldSettings.SpawnPosition);

        int count = _currentFigure.Length;
        for (int i = 0; i < count; i++)
        {
            _currentFigure[i].Column += _fieldSettings.SpawnPosition.Column;
            _currentFigure[i].Row += _fieldSettings.SpawnPosition.Row;
            AddBlock(_currentFigure[i]);
        }
        fallTimer = 0f;
    }
    private void AddBlock(MatrixPosition position)
    {
        _field[position.Column, position.Row] = true;
    }
    private void ClearBlock(MatrixPosition position)
    {
        _field[position.Column, position.Row] = false;
    }
}
