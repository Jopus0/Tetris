using UnityEngine;

[CreateAssetMenu(fileName = "new FieldSettings", menuName = "FieldSettings")]
public class FieldSettings : ScriptableObject
{
    public int Width;
    public int Height;
    public int StartHeight;
    public MatrixPosition SpawnPosition;
}
