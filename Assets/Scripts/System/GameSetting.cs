using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "Scriptable Objects/GameSetting")]
public class GameSetting : ScriptableObject
{
    public int BoardWidth = 8;
    public int BoardHeight = 8;
    public int MatchMin = 3;
    public float LevelTime = 300f; // in seconds
    public float TimeForHint = 5f;
}
