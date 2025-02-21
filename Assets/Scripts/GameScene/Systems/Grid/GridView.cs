using UnityEngine;

public class GridView
{
    CellFactory _cellFactory;
    Transform _cellTransform;
    public GridView(CellFactory cellFactory, Transform cellTransform)
    {
        _cellFactory = cellFactory;
        _cellTransform = cellTransform;   
    }
    public void CreateGrid(Vector2[,] grid, int height, int width, int startHeight, float cellSize)
    {
        for (int i = startHeight; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var cell = _cellFactory.Create();
                cell.transform.parent = _cellTransform;
                cell.transform.position = grid[i, j];
                cell.transform.localScale = new Vector2(cellSize, cellSize);
            }
        }
    }
}
