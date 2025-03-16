using UnityEngine;

[CreateAssetMenu(fileName = "new FigureMoverSettngs", menuName = "FigureMoverSettngs")]
public class FigureMoverSettngs : ScriptableObject
{
    public float BaseFallTime;
    public float MinFallTime;
    public float FallScale;
    public float BaseMoveTime;
    public float MinMoveTime;
    public float MoveScale;
    public float LandTime;
}
