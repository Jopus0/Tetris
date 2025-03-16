using UnityEngine;

[CreateAssetMenu(fileName = "new FigureSettings", menuName = "FigureSettings")]
public class FigureSettings : ScriptableObject
{
    public MatrixPosition[] Figure;
    public Color Color;
    public Sprite FigureImage;
    public MatrixPosition[] GetFigureCopy()
    {
        return (MatrixPosition[])Figure.Clone();
    }
}
