using System.Collections.Generic;
using System;
using Zenject;
public class Field : IInitializable, ITickable
{
    public event Action<int> OnLinesCleared;

    private FieldSettings _fieldSettings;
    private FieldView _fieldView;

    private bool[,] _field;
    public Field(FieldSettings fieldSettings, FieldView fieldView)
    {
        _fieldSettings = fieldSettings;
        _fieldView = fieldView;
    }
    public void Initialize()
    {
        _field = new bool[_fieldSettings.Height, _fieldSettings.Width];
        _fieldView.GenerateGrid(_fieldSettings.Height, _fieldSettings.Width, _fieldSettings.StartHeight);
    }
    public void Tick()
    {
        ClearLines();
    }
    public MatrixPosition GetSize()
    {
        return new MatrixPosition(_fieldSettings.Height, _fieldSettings.Width);
    }
    public void OnFigureCreate(FigureSettings figureSettings, MatrixPosition[] figure)
    {
        _fieldView.CreateFigure(figureSettings, figure);
    }
    public void OnFigureMove(List<MatrixPosition> prevPositions, List<MatrixPosition> newPositions)
    {
        _fieldView.MoveBlocks(prevPositions, newPositions);
    }
    public void OnFigureLanded(MatrixPosition[] figure)
    {
        foreach(var block in figure)
            _field[block.Row, block.Column] = true;
    }
    public bool IsPositionValid(MatrixPosition position)
    {
        bool rowValid = position.Row >= 0 && position.Row < _fieldSettings.Height;
        bool columnValid = position.Column >= 0 && position.Column < _fieldSettings.Width;
        return columnValid && rowValid && IsPositionEmpty(position);
    }
    public bool IsPositionEmpty(MatrixPosition position)
    {
        return !_field[position.Row, position.Column];
    }
    private void ClearLines()
    {
        List<int> clearedRows = new List<int>();
        List<MatrixPosition> clearList;
        for (int i = _fieldSettings.Height - 1; i >= _fieldSettings.StartHeight; i--)
        {
            if (IsRowFull(i))
            {
                clearList = new List<MatrixPosition>();
                for (int j = 0; j < _fieldSettings.Width; j++)
                {
                    _field[i, j] = false;
                    clearList.Add(new MatrixPosition(i, j));
                }
                clearedRows.Add(i);
                _fieldView.DestroyBlocks(clearList);
            }
        }
        if(clearedRows.Count > 0)
        {
            SlideDown(clearedRows);
            OnLinesCleared?.Invoke(clearedRows.Count);
        }
    }
    private void SlideDown(List<int> clearedRows)
    {
        List<MatrixPosition> prevPositions = new List<MatrixPosition>();
        List<MatrixPosition> newPositions = new List<MatrixPosition>();

        int count = clearedRows.Count;
        int shift = 0;
        for (int i = _fieldSettings.Height - 1; i >= 0; i--)
        {
            if (shift < count && clearedRows[shift] == i)
            {
                shift++;
            }
            else
            {
                for (int j = 0; j < _fieldSettings.Width; j++)
                {
                    if (_field[i, j])
                    {
                        _field[i, j] = false;
                        _field[i + shift, j] = true;
                        prevPositions.Add(new MatrixPosition(i, j));
                        newPositions.Add(new MatrixPosition(i + shift, j));
                    }
                }
            }
        }
        _fieldView.MoveBlocks(prevPositions, newPositions);
    }
    private bool IsRowFull(int row)
    {
        for(int i = 0; i < _fieldSettings.Width; i++)
        {
            if (!_field[row, i])
            {
                return false;
            }
        }
        return true;
    }
}
