using UnityEngine;
public class Grid
{
    public Vector2[,] GenerateGrid(int height, int width, int startHeight, Vector2 centerPosition, float cellSize)
    {
        float startOffset = cellSize / 2f;
        float startX = centerPosition.x - (width / 2f) * cellSize - startOffset;
        float startY = centerPosition.y + (height / 2f) * cellSize - startOffset;

        var grid = new Vector2[height, width];
        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                grid[i,j] = new Vector2(startX + (cellSize * j), startY - (cellSize * i));
            }
        }
        return grid;
    }
}
