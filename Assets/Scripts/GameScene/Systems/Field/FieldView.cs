using System.Collections.Generic;
using Zenject;
using UnityEngine;
public class FieldView : MonoBehaviour
{
    [SerializeField] private Transform _blockParent;
    [SerializeField] private Transform _cellParent;

    private FieldViewSettings _fieldViewSettings;
    private Camera _mainCamera;
    private Grid _grid;
    private BlockFactory _blockFactory;
    private CellFactory _cellFactory;

    private Vector2[,] _gridPoints;
    private float _cellSize;

    private Block[] _currentFigure;
    private Dictionary<MatrixPosition, Block> _field;

    [Inject]
    private void Construct(FieldViewSettings fieldViewSettings, Camera mainCamera, Grid grid, BlockFactory blockFactory, CellFactory cellFactory)
    {
        _fieldViewSettings = fieldViewSettings;
        _mainCamera = mainCamera;
        _grid = grid;
        _blockFactory = blockFactory;
        _cellFactory = cellFactory;
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
        CreateGrid(_gridPoints, height, width, startHeight, _cellSize);
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
            _currentFigure[i].transform.position = _gridPoints[row, column];
            float blocksize = _cellSize * _fieldViewSettings.BlockSizeScale;
            _currentFigure[i].transform.localScale = new Vector2(blocksize, blocksize);

            _field.Add(new MatrixPosition(row, column), _currentFigure[i]);
        }
    }
    public void MoveBlocks(List<MatrixPosition> prevPositions, List<MatrixPosition> newPositions)
    {
        int count = prevPositions.Count;
        Block[] blocks = new Block[count];

        for (int i = 0; i < count; i++)
        {
            _field.TryGetValue(prevPositions[i], out blocks[i]);
            _field.Remove(prevPositions[i]);
        }

        for (int i = 0; i < count; i++)
        {
            _field.Add(newPositions[i], blocks[i]);
            blocks[i].transform.position = _gridPoints[newPositions[i].Row, newPositions[i].Column];
        }
    }
    public void DestroyBlocks(List<MatrixPosition> positions)
    {
        Block block;
        foreach(var position in positions)
        {
            _field.TryGetValue(position, out block);
            _field.Remove(position);
            Destroy(block.gameObject);
        }
    }
    private void CreateGrid(Vector2[,] grid, int height, int width, int startHeight, float cellSize)
    {
        for (int i = startHeight; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var cell = _cellFactory.Create();
                cell.transform.parent = _cellParent;
                cell.transform.position = new Vector3(grid[i, j].x, grid[i, j].y, 1f);
                cell.transform.localScale = new Vector2(cellSize, cellSize);
            }
        }
    }
}
