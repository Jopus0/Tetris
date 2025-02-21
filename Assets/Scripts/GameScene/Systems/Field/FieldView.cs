using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.Rendering.DebugUI;
public class FieldView
{
    private FieldViewSettings _fieldViewSettings;
    private Camera _mainCamera;
    private Grid _grid;
    private BlockFactory _blockFactory;

    private Vector2[,] _gridPoints;
    private float _cellSize;

    private Block[] _currentFigure;
    private Dictionary<MatrixPosition, Block> _field;
    public FieldView(FieldViewSettings fieldViewSettings, Camera mainCamera, Grid grid, BlockFactory blockFactory)
    {
        _fieldViewSettings = fieldViewSettings;
        _mainCamera = mainCamera;
        _grid = grid;
        _blockFactory = blockFactory;
    }
    public void GenerateGrid(int height, int width, int startHeight)
    {
        _field = new Dictionary<MatrixPosition, Block>();

        float centerX = _fieldViewSettings.CenterPositionPercentage.x * Screen.width;
        float centerY = _fieldViewSettings.CenterPositionPercentage.y * Screen.height;
        Vector2 centerPosition = _mainCamera.ScreenToWorldPoint(new Vector2(centerX, centerY));

        int minSide = Screen.width < Screen.height ? Screen.width : Screen.height;
        float cellSizeInPixels = _fieldViewSettings.CellSizePercentage * minSide;
        Vector2 cellStartPosition = _mainCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 cellOffsetPosition = _mainCamera.ScreenToWorldPoint(new Vector2(cellSizeInPixels, 0f));
        _cellSize = cellOffsetPosition.x - cellStartPosition.x;

        _gridPoints = _grid.GenerateGrid(height, width, startHeight, centerPosition, _cellSize);
    }
    public void MoveBlocks(MatrixPosition[] prevPositions, MatrixPosition[] newPositions)
    {
        int count = prevPositions.Length;
        Block[] blocks = new Block[count];
        for (int i = 0; i < count; i++)
        {
            _field.TryGetValue(prevPositions[i], out blocks[i]);
            ClearBlock(prevPositions[i]);
        }
        for (int i = 0; i < count; i++)
        {
            AddBlock(newPositions[i], blocks[i]);
            blocks[i].transform.position = _gridPoints[newPositions[i].Column, newPositions[i].Row];
        }
    }
    public void CreateFigure(FigureSettings figureSettings, MatrixPosition spawnPosition)
    {
        var figure = figureSettings.GetFigureCopy();
        int blockCount = figure.Length;
        Color blockColor = figureSettings.Color;

        _currentFigure = new Block[blockCount];
        for (int i = 0; i < blockCount; i++)
        {
            _currentFigure[i] = _blockFactory.Create();
            _currentFigure[i].SpriteRenderer.color = blockColor;

            int column = spawnPosition.Column + figure[i].Column;
            int row = spawnPosition.Row + figure[i].Row;
            _currentFigure[i].transform.position = _gridPoints[column, row];
            _currentFigure[i].transform.localScale = new Vector2(_cellSize, _cellSize);

            AddBlock(new MatrixPosition(column, row), _currentFigure[i]);
        }
    }
    private void AddBlock(MatrixPosition position, Block block)
    {
        _field.Add(position, block);
    }
    private void ClearBlock(MatrixPosition position)
    {
        _field.Remove(position);
    }
}
