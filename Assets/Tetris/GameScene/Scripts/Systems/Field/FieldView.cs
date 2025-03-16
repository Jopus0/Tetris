using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System;
public class FieldView : MonoBehaviour
{
    [SerializeField] private Transform _pointParent;
    [SerializeField] private Transform _blockParent;
    [SerializeField] private Transform _cellParent;

    private FieldViewSettings _settings;
    private Field _field;
    private Camera _mainCamera;
    private BlockFactory _blockFactory;
    private CellFactory _cellFactory;

    private Transform[,] _gridPoints;
    private float _cellSize;

    private Block[] _currentFigure;
    private Dictionary<MatrixPosition, Block> _blocks;

    [Inject]
    private void Construct(FieldViewSettings settings, Field field, Camera mainCamera, BlockFactory blockFactory, CellFactory cellFactory)
    {
        _settings = settings;
        _field = field;
        _mainCamera = mainCamera;
        _blockFactory = blockFactory;
        _cellFactory = cellFactory;

        _field.OnFieldCreated += GenerateGrid;
        _field.OnBlocksCreated += CreateFigure;
        _field.OnBlocksMoved += MoveBlocks;
        _field.OnBlocksDestroyed += DestroyBlocks;
    }
    public void OnDestroy()
    {
        _field.OnFieldCreated -= GenerateGrid;
        _field.OnBlocksCreated -= CreateFigure;
        _field.OnBlocksMoved -= MoveBlocks;
        _field.OnBlocksDestroyed -= DestroyBlocks;
    }
    public void GenerateGrid(FieldSettings settings)
    {
        _blocks = new Dictionary<MatrixPosition, Block>();

        int minSide = Screen.width < Screen.height ? Screen.width : Screen.height;
        float cellSizeInPixels = _settings.CellSizePercentage * minSide;
        Vector2 cellStartPosition = _mainCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 cellOffsetPosition = _mainCamera.ScreenToWorldPoint(new Vector2(cellSizeInPixels, 0f));
        _cellSize = cellOffsetPosition.x - cellStartPosition.x;

        _gridPoints = new Transform[settings.Height, settings.Width];

        float startOffset = _cellSize / 2f;
        float startX = transform.position.x - (settings.Width / 2f) * _cellSize + startOffset;
        float startY = transform.position.y + (settings.Height / 2f) * _cellSize - startOffset;

        for (int i = 0; i < settings.Height; i++)
        {
            for (int j = 0; j < settings.Width; j++)
            {
                Transform point = new GameObject($"point[{i},{j}]").transform;
                point.parent = _pointParent;
                point.transform.position = new Vector3(startX + (_cellSize * j), startY - (_cellSize * i));
                _gridPoints[i, j] = point;
            }
        }
        CreateGrid(_gridPoints, settings.Height, settings.Width, settings.StartHeight, _cellSize);
    }
    public void CreateFigure(FigureSettings figureSettings, MatrixPosition[] figure)
    {
        int blockCount = figure.Length;
        Color blockColor = figureSettings.Color;

        _currentFigure = new Block[blockCount];
        for (int i = 0; i < blockCount; i++)
        {
            _currentFigure[i] = _blockFactory.Create();
            _currentFigure[i].SpriteRenderer.color = blockColor;

            int row = figure[i].Row;
            int column = figure[i].Column;
            _currentFigure[i].transform.parent = _blockParent;
            _currentFigure[i].transform.position = _gridPoints[row, column].position;
            float blocksize = _cellSize * _settings.BlockSizeScale;
            _currentFigure[i].transform.localScale = new Vector2(blocksize, blocksize);

            _blocks.Add(new MatrixPosition(row, column), _currentFigure[i]);
        }
    }
    public void MoveBlocks(List<MatrixPosition> prevPositions, List<MatrixPosition> newPositions)
    {
        int count = prevPositions.Count;
        Block[] blocks = new Block[count];

        for (int i = 0; i < count; i++)
        {
            _blocks.TryGetValue(prevPositions[i], out blocks[i]);
            _blocks.Remove(prevPositions[i]);
        }

        for (int i = 0; i < count; i++)
        {
            _blocks.Add(newPositions[i], blocks[i]);
            blocks[i].transform.position = _gridPoints[newPositions[i].Row, newPositions[i].Column].position;
        }
    }
    public void DestroyBlocks(List<MatrixPosition> positions)
    {
        Block block;
        foreach(var position in positions)
        {
            _blocks.TryGetValue(position, out block);
            _blocks.Remove(position);
            Destroy(block.gameObject);
        }
    }
    private void CreateGrid(Transform[,] grid, int height, int width, int startHeight, float cellSize)
    {
        for (int i = startHeight; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var cell = _cellFactory.Create();
                cell.name = $"cell[{i},{j}]";
                cell.transform.parent = _cellParent;
                cell.transform.position = new Vector3(grid[i, j].position.x, grid[i, j].position.y, 1f);
                cell.transform.localScale = new Vector2(cellSize, cellSize);
            }
        }
    }
}
