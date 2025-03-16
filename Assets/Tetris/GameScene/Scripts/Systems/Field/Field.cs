using System.Collections.Generic;
using System;
using Zenject;
public class Field : IInitializable
{
    public event Action<int> OnLinesCleared;

    public event Action<FieldSettings> OnFieldCreated;
    public event Action<FigureSettings, MatrixPosition[]> OnBlocksCreated;
    public event Action<List<MatrixPosition>, List<MatrixPosition>>  OnBlocksMoved;
    public event Action<List<MatrixPosition>> OnBlocksDestroyed;
    public event Action OnBlocksLocked;
    public event Action OnBlockOutOffZone;
    public FieldSettings Settings { get; private set; }

    private bool[,] _matrix;
    private List<MatrixPosition> _positionsList1;
    private List<MatrixPosition> _positionsList2;
    private int _height;
    private int _width;
    private int _startHeight;
    public Field(FieldSettings settings)
    {
        Settings = settings;

        _matrix = new bool[Settings.Height, Settings.Width];
        _positionsList1 = new List<MatrixPosition>();
        _positionsList2 = new List<MatrixPosition>();
        _height = Settings.Height;
        _width = Settings.Width;
        _startHeight = Settings.StartHeight;
    }
    public void Initialize()
    {
        OnFieldCreated?.Invoke(Settings);
    }
    public void CreateBlocks(FigureSettings figureSettings, MatrixPosition[] blocks)
    {
        OnBlocksCreated?.Invoke(figureSettings, blocks);
    }
    public void TryMoveBlocks(ref MatrixPosition[] blocks, MatrixPosition deltaPosition)
    {
        int count = blocks.Length;
        MatrixPosition[] newBlocks = new MatrixPosition[count];
        for (int i = 0; i < count; i++)
        {
            _positionsList1.Add(blocks[i]);
            newBlocks[i] = blocks[i] + deltaPosition;
            _positionsList2.Add(newBlocks[i]);

            if (!IsPositionValid(newBlocks[i]))
            {
                ClearLists();
                return;
            }
        }
        blocks = newBlocks;

        OnBlocksMoved?.Invoke(_positionsList1, _positionsList2);
        ClearLists();
    }
    public void TryMoveBlocks(ref MatrixPosition[] blocks, MatrixPosition[] deltaPositions)
    {
        int count = blocks.Length;
        MatrixPosition[] newBlocks = new MatrixPosition[count];
        for (int i = 0; i < count; i++)
        {
            _positionsList1.Add(blocks[i]);
            newBlocks[i] = blocks[i] + deltaPositions[i];
            _positionsList2.Add(newBlocks[i]);

            if (!IsPositionValid(newBlocks[i]))
            {
                ClearLists();
                return;
            }
        }
        blocks = newBlocks;

        OnBlocksMoved?.Invoke(_positionsList1, _positionsList2);
        ClearLists();
    }
    public void DropBlocks(ref MatrixPosition[] blocks)
    {
        int deltaRows = 0;
        while (!IsBlocksLanded(blocks, deltaRows))
        {
            deltaRows++;
        }
        TryMoveBlocks(ref blocks, new MatrixPosition(deltaRows, 0));
    }
    public bool IsBlocksLanded(MatrixPosition[] blocks, int deltaRows)
    {
        foreach (var block in blocks)
        {
            int lowerRow = block.Row + 1 + deltaRows;
            if (lowerRow == _height || _matrix[lowerRow, block.Column])
                return true;
        }
        return false;
    }
    public void LockBlocks(MatrixPosition[] blocks)
    {
        if (IsOutOffZone(blocks))
            OnBlockOutOffZone?.Invoke();

        foreach (var block in blocks)
            _matrix[block.Row, block.Column] = true;

        OnBlocksLocked?.Invoke();
    }
    public void ClearLines()
    {
        List<int> clearedRows = new List<int>();
        for (int i = _height - 1; i >= _startHeight; i--)
        {
            if (IsRowFull(i))
            {
                for (int j = 0; j < _width; j++)
                {
                    _matrix[i, j] = false;
                    _positionsList1.Add(new MatrixPosition(i, j));
                }
                clearedRows.Add(i);
            }
        }
        if(clearedRows.Count > 0)
        {
            OnBlocksDestroyed?.Invoke(_positionsList1);
            _positionsList1.Clear();
            SlideDown(clearedRows);
            OnLinesCleared?.Invoke(clearedRows.Count);
        }
    }
    private void SlideDown(List<int> clearedRows)
    {
        int count = clearedRows.Count;
        int shift = 0;
        for (int i = _height - 1; i >= 0; i--)
        {
            if (shift < count && clearedRows[shift] == i)
            {
                shift++;
            }
            else
            {
                for (int j = 0; j < _width; j++)
                {
                    if (_matrix[i, j])
                    {
                        _matrix[i, j] = false;
                        _matrix[i + shift, j] = true;
                        _positionsList1.Add(new MatrixPosition(i, j));
                        _positionsList2.Add(new MatrixPosition(i + shift, j));
                    }
                }
            }
        }
        OnBlocksMoved?.Invoke(_positionsList1, _positionsList2);
        ClearLists();
    }
    private bool IsPositionValid(MatrixPosition position)
    {
        bool rowValid = position.Row >= 0 && position.Row < _height;
        bool columnValid = position.Column >= 0 && position.Column < _width;
        return rowValid && columnValid && !_matrix[position.Row, position.Column];
    }
    private bool IsRowFull(int row)
    {
        for(int i = 0; i < _width; i++)
        {
            if (!_matrix[row, i])
                return false;
        }
        return true;
    }
    private bool IsOutOffZone(MatrixPosition[] blocks)
    {
        foreach (var block in blocks)
        {
            if (block.Row < _startHeight)
                return true;
        }
        return false;
    }
    private void ClearLists()
    {
        _positionsList1.Clear();
        _positionsList2.Clear();
    }
}
