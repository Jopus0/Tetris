using UnityEngine;

[CreateAssetMenu(fileName = "new LevelHandlerSettings", menuName = "LevelHandlerSettings")]
public class LevelHandlerSettings : ScriptableObject
{
    public int LinesForLevel;
    public LevelComplication[] LevelComplications;
}