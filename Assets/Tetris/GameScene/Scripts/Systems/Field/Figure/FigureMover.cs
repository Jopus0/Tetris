using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class FigureMover : IInitializable, IDisposable ,ITickable
{
    private FigureMoverSettngs _settngs;
    private IInput _input;
    private CoroutinePerformer _coroutinePerformer;

    private Field _field;
    private MatrixPosition[] _currentFigure;

    private float _baseFallTime;
    private float _fallTime;
    private float _minFallTime;
    private float _fallScale;

    private float _baseMoveTime;
    private float _moveTime;
    private float _minMoveTime;
    private float _moveScale;

    private float _landTime;

    private float _fallTimer;
    private bool _isFallAceleration;

    private float _horizontalMoveTimer;
    private int _horizontalDirections;
    private int _horizontalMoveDirection;
    private bool _isHorizontalMoving;

    private bool _isFalling;
    private bool _isLanded;

    private Coroutine _landTimerCoroutine;
    public FigureMover(FigureMoverSettngs settngs, Field field, IInput input, CoroutinePerformer coroutinePerformer)
    {
        _settngs = settngs;
        _field = field;
        _input = input;
        _coroutinePerformer = coroutinePerformer;

        _baseFallTime = _settngs.BaseFallTime;
        _fallTime = _baseFallTime;
        _minFallTime = _settngs.MinFallTime;
        _fallScale = _settngs.FallScale;
        _baseMoveTime = _settngs.BaseMoveTime;
        _moveTime = _baseMoveTime;
        _minMoveTime = _settngs.MinMoveTime;
        _moveScale = _settngs.MoveScale;
        _landTime = _settngs.LandTime;

        _horizontalMoveTimer = 0f;
        _horizontalDirections = 0;
        _horizontalMoveDirection = 0;
        _isHorizontalMoving = false;
        _isFalling = true;
        _isLanded = false;
    }
    public void Initialize()
    {
        _input.OnFallButtonDown += StartFallAcceleration;
        _input.OnFallButtonUp += EndFallAcceleration;
        _input.OnMoveButtonDown += StartHorizontalMove;
        _input.OnMoveButtonUp += EndHorizontalMove;
        _input.OnRotateButtonDown += Rotate;
        _input.OnHardDropButtonDown += HardDrop;
    }
    public void Dispose()
    {
        _input.OnFallButtonDown -= StartFallAcceleration;
        _input.OnFallButtonUp -= EndFallAcceleration;
        _input.OnMoveButtonDown -= StartHorizontalMove;
        _input.OnMoveButtonUp -= EndHorizontalMove;
        _input.OnRotateButtonDown -= Rotate;
        _input.OnHardDropButtonDown -= HardDrop;

        if (_coroutinePerformer != null && _landTimerCoroutine != null)
            _coroutinePerformer.StopCoroutine(_landTimerCoroutine);
    }
    public void Tick()
    {
        if (_currentFigure == null)
            return;

        if (_isHorizontalMoving)
            HorizontalMove();

        if (_isFallAceleration)
            FallAceleration();

        Fall();

        CheckLandFigure();
    }
    public void SetFigure(MatrixPosition[] figure)
    {
        _currentFigure = figure;
        _isFalling = true;
        _isLanded = false;
    }
    public void SetBaseFallTime(float time)
    {
        _baseFallTime = time;
        _fallTime = Mathf.Min(_baseFallTime, _fallTime);
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

        if (_currentFigure != null)
            _field.TryMoveBlocks(ref _currentFigure, new MatrixPosition(0, _horizontalMoveDirection));

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

        int left = _currentFigure[0].Column;
        int upper = _currentFigure[0].Row;
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

        int cellSize = Mathf.Max(lower - upper, right - left);

        int leftColumn = Mathf.RoundToInt((totalColumn / count) - (cellSize / 2f));
        int upperRow = Mathf.RoundToInt((totalRow / count) - (cellSize / 2f));

        MatrixPosition[] deltaPositions = new MatrixPosition[count];
        for (int i = 0; i < count; i++)
        {
            deltaPositions[i].Column =  (leftColumn + Mathf.Abs((_currentFigure[i].Row - upperRow) - cellSize)) - _currentFigure[i].Column;
            deltaPositions[i].Row =  (upperRow + (_currentFigure[i].Column - leftColumn)) - _currentFigure[i].Row;
        }

        _field.TryMoveBlocks(ref _currentFigure, deltaPositions);
    }
    private void Fall()
    {
        if (!_isFalling)
            return;

        if (_fallTimer >= _fallTime)
        {
            _field.TryMoveBlocks(ref _currentFigure, new MatrixPosition(1, 0));
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
            _field.TryMoveBlocks(ref _currentFigure, new MatrixPosition(0, _horizontalMoveDirection));
            _horizontalMoveTimer = 0f;
        }
        else
        {
            _horizontalMoveTimer += Time.deltaTime;
        }
    }
    public void HardDrop()
    {
        if (_currentFigure == null)
            return;

        _isLanded = true;
        _isFalling = false;

        if (_landTimerCoroutine != null)
            _coroutinePerformer.StopCoroutine(_landTimerCoroutine);

        _field.DropBlocks(ref _currentFigure);
        _field.LockBlocks(_currentFigure);
    }
    private void CheckLandFigure()
    {
        if (!_isLanded && _field.IsBlocksLanded(_currentFigure, 0))
        {
            _isLanded = true;
            _landTimerCoroutine = _coroutinePerformer.StartCoroutine(LandTimer());
        }
    }
    private void LandFigure()
    {
        _field.LockBlocks(_currentFigure);
    }
    private IEnumerator LandTimer()
    {
        _isFalling = false;
        yield return new WaitForSeconds(_landTime);
        LandFigure();
    }
}
