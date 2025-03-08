using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/new GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Game settings")]
    [SerializeField] private int _targetFrameRate = 30;
    [SerializeField] private int _vSyncCount = 0;

    public int GameTargetFrameRate
    {
        get => _targetFrameRate;
    }
    public int GameVSyncCount
    {
        get => _vSyncCount;
    }
}