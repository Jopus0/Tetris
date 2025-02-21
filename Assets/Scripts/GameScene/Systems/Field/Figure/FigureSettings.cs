using UnityEngine;

[CreateAssetMenu(fileName = "new FigureSettings", menuName = "FigureSettings")]
public class FigureSettings : ScriptableObject
{
    public MatrixPosition[] Figure;
    public Color Color;

    public MatrixPosition[] GetFigureCopy()
    {
        return (MatrixPosition[])Figure.Clone();
    }
}
