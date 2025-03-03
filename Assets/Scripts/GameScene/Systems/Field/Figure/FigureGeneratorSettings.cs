using UnityEngine;

[CreateAssetMenu(fileName = "new FigureGeneratorSettings", menuName = "FigureGeneratorSettings")]
public class FigureGeneratorSettings : ScriptableObject
{
    public FigureSettings[] FiguresSettings;
}
