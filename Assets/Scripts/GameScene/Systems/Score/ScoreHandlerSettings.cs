using UnityEngine;

[CreateAssetMenu(fileName = "new ScoreHandlerSettings", menuName = "ScoreHandlerSettings")]
public class ScoreHandlerSettings : ScriptableObject
{
    public LineScore[] LinesScore;
}