using System;

[Serializable]
public struct MatrixPosition
{
    public int Row;
    public int Column;
    public MatrixPosition(int row = 0, int column = 0)
    {
        Row = row;
        Column = column;
    }
}
