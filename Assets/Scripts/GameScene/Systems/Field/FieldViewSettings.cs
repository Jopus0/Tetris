using UnityEngine;

[CreateAssetMenu(fileName = "new FieldViewSettings", menuName = "FieldViewSettings")]
public class FieldViewSettings : ScriptableObject
{
    public Vector2 CenterPositionPercentage;
    public float CellSizePercentage;
    public float BlockSizeScale;
}
