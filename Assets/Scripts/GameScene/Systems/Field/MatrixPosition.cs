using System;

[Serializable]
public struct MatrixPosition
{
    public int Column;
    public int Row;
    public MatrixPosition(int column = 0, int row = 0)
    {
        Column = column;
        Row = row;
    }
}
