using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FigureMover : IInitializable, IDisposable ,ITickable
{
    private FigureMoverSettngs _settngs;
    private IInput _input;
    private Field _field;
    private FigureGenerator _figureGenerator;

    private MatrixPosition[] _currentFigure;
    private MatrixPosition _fieldSize;

    private float _baseFallTime;
    private float _fallTime;
    private float _minFallTime;
    private float _fallScale;

    private float _baseMoveTime;
    private float _moveTime;
    private float _minMoveTime;
    private float _moveScale;

    private float _fallTimer;
    private bool _isFallAceleration;

    private float _horizontalMoveTimer;
    private int _horizontalDirections;
    private int _horizontalMoveDirection;
    private bool _isHorizontalMoving;
    public FigureMover(FigureMoverSettngs settngs, IInput input, Field field, FigureGenerator figureGenerator)
    {
        _settngs = settngs;
        _input = input;
        _field = field;
        _figureGenerator = figureGenerator;

        _baseFallTime = settngs.BaseFallTime;
        _fallTime = _baseFallTime;
        _minFallTime = settngs.MinFallTime;
        _fallScale = settngs.FallScale;
        _baseMoveTime = settngs.BaseMoveTime;
        _moveTime = _baseMoveTime;
        _minMoveTime = settngs.MinMoveTime;
        _moveScale = settngs.MoveScale;

        _horizontalDirections = 0;
        _fieldSize = _field.GetSize();
    }
    public void Initialize()
    {
        _input.OnFallButtonDown += StartFallAcceleration;
        _input.OnFallButtonUp += EndFallAcceleration;
        _input.OnMoveButtonDown += StartHorizontalMove;
        _input.OnMoveButtonUp += EndHorizontalMove;
        _input.OnRotateButtonDown += Rotate;
        CreateFigure();
    }
    public void Dispose()
    {
        _input.OnFallButtonDown -= StartFallAcceleration;
        _input.OnFallButtonUp -= EndFallAcceleration;
        _input.OnMoveButtonDown -= StartHorizontalMove;
        _input.OnMoveButtonUp -= EndHorizontalMove;
        _input.OnRotateButtonDown -= Rotate;
    }
    public void Tick()
    {
        if (_currentFigure == null)
            return;

        if (_isFallAceleration)
            FallAceleration();

        Fall();

        if (_isHorizontalMoving)
            HorizontalMove();

        if (IsLanded())
        {
            _field.OnFigureLanded(_currentFigure);
            CreateFigure();
        }
    }
    public void SetBaseFallTime(float time)
    {
        _baseFallTime = time;
    }
    public void CreateFigure()
    {
        FigureSettings figureSettings = _figureGenerator.GetRandomFigure();
        _currentFigure = figureSettings.GetFigureCopy();

        int count = _currentFigure.Length;
        for (int i = 0; i < count; i++)
        {
            _currentFigure[i].Column += _settngs.SpawnPosition.Column;
            _currentFigure[i].Row += _settngs.SpawnPosition.Row;
        }
        _field.OnFigureCreate(figureSettings, _currentFigure);
    }
    public void StartFallAcceleration()
    {
        _isFallAceleration = true;
    }
    public void EndFallAcceleration()
    {
        _isFallAceleration = false;
        _fallTime = _baseFallTime;
    }
    public void StartHorizontalMove(int direction)
    {
        _horizontalDirections++;

        _horizontalMoveDirection = direction;
        Move(_currentFigure, 0, _horizontalMoveDirection);
        _isHorizontalMoving = true;
    }
    public void EndHorizontalMove()
    {
        _horizontalDirections--;
        if (_horizontalDirections != 0)
        {
            _moveTime = _baseMoveTime;
            return;
        }

        _isHorizontalMoving = false;
        _moveTime = _baseMoveTime;
    }
    public void Rotate()
    {
        if (_currentFigure == null)
            return;

        int count = _currentFigure.Length;

        int left = _fieldSize.Column;
        int upper = _fieldSize.Row;
        int right = 0;
        int lower = 0;

        float totalRow = 0;
        float totalColumn = 0;

        foreach (var block in _currentFigure)
        {
            left = Mathf.Min(block.Column, left);
            right = Mathf.Max(block.Column, right);

            upper = Mathf.Min(block.Row, upper);
            lower = Mathf.Max(block.Row, lower);

            totalRow += block.Row;
            totalColumn += block.Column;
        }

        int cellSize = Mathf.Max(right - left, lower - upper);

        int leftColumn = Mathf.RoundToInt((totalColumn / count) - (cellSize / 2f));
        int upperRow = Mathf.RoundToInt((totalRow / count) - (cellSize / 2f));

        List<MatrixPosition> prevPositions = new List<MatrixPosition>();
        List<MatrixPosition> newPositions = new List<MatrixPosition>();
        MatrixPosition[] newFigure = new MatrixPosition[count];
        for (int i = 0; i < count; i++)
        {
            prevPositions.Add(_currentFigure[i]);
            newFigure[i].Column = leftColumn + (_currentFigure[i].Row - upperRow);
            newFigure[i].Row = upperRow + Mathf.Abs((_currentFigure[i].Column - leftColumn) - cellSize);
            newPositions.Add(newFigure[i]);

            if (!_field.IsPositionValid(newPositions[i]))
                return;
        }

        _currentFigure = newFigure;
        _field.OnFigureMove(prevPositions, newPositions);
    }
    private void Fall()
    {
        if (_fallTimer >= _fallTime)
        {
            Move(_currentFigure, 1, 0);
            _fallTimer = 0f;
        }
        else
        {
            _fallTimer += Time.deltaTime;
        }
    }
    private void FallAceleration()
    {
        if (_fallTime > _minFallTime)
        {
            _fallTime -= (_fallScale * Time.deltaTime);
            if (_fallTime < _minFallTime)
                _fallTime = _minFallTime;
        }
    }
    private void HorizontalMove()
    {
        if(_moveTime > _minMoveTime)
        {
            _moveTime -= (_moveScale * Time.deltaTime);
            if(_moveTime < _minMoveTime)
                _moveTime = _minMoveTime;
        }

        if (_horizontalMoveTimer >= _moveTime)
        {
            Move(_currentFigure, 0, _horizontalMoveDirection);
            _horizontalMoveTimer = 0f;
        }
        else
        {
            _horizontalMoveTimer += Time.deltaTime;
        }
    }
    private void Move(MatrixPosition[] figure, int deltaColumns, int deltaRows)
    {
        int count = figure.Length;
        List<MatrixPosition> prevPositions = new List<MatrixPosition>();
        List<MatrixPosition> newPositions = new List<MatrixPosition>();
        MatrixPosition[] newFigure = new MatrixPosition[count];
        for (int i = 0; i < count; i++)
        {
            prevPositions.Add(figure[i]);
            newFigure[i].Column = figure[i].Column + deltaColumns;
            newFigure[i].Row = figure[i].Row + deltaRows;
            newPositions.Add(newFigure[i]);

            if (!_field.IsPositionValid(newPositions[i]))
                return;
        }
        _currentFigure = newFigure;
        _field.OnFigureMove(prevPositions, newPositions);
    }
    private bool IsLanded()
    {
        int count = _currentFigure.Length;

        foreach (var block in _currentFigure)
        {
            int lowerColumn = block.Column + 1;
            if (lowerColumn == _fieldSize.Column || !_field.IsPositionEmpty(new MatrixPosition(lowerColumn, block.Row)))
            {
                return true;
            }
        }
        return false;
    }
}
