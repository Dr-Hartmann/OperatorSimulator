using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/new GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Game settings")]
    [SerializeField] private int _targetFrameRate = 30;
    [SerializeField] private int _vSyncCount = 0;

    private void Awake()
    {
        GameSetFrameRate();
    }

    //TODO - задел на будущее
    public int GameTargetFrameRate
    {
        get => _targetFrameRate;
    }
    public int GameVSyncCount
    {
        get => _vSyncCount;
    }
    public void GameSetFrameRate()
    {
        QualitySettings.vSyncCount = _vSyncCount;
        Application.targetFrameRate = _targetFrameRate;
    }
}