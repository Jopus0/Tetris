using UnityEngine;

[CreateAssetMenu(fileName = "new FigureMoverSettngs", menuName = "FigureMoverSettngs")]
public class FigureMoverSettngs : ScriptableObject
{
    public MatrixPosition SpawnPosition;

    public float BaseFallTime;
    public float MinFallTime;
    public float FallScale;
    public float BaseMoveTime;
    public float MinMoveTime;
    public float MoveScale;
}
