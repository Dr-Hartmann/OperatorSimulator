using Dialogues.System;
using PlayerSpace;
using Simulation;
using System;
using System.Collections;
using Tips;
using UnityEngine;
using UserInterface;

public class EntryPointGameplay : MonoBehaviour
{
    [Header("Simulation")]
    [SerializeField] private SimulationSettings _simulationSettings;

    [Header("Game")]
    [SerializeField] private GameSettings _gameSettings;

    [Header("UI")]
    [SerializeField] private UISettings _uiSettings;

    [Header("Tip")]
    [SerializeField] private TipSettings _tipSettings;
    [SerializeField] private RectTransform _tipsPlace;

    [Header("Dialog")]
    [SerializeField] private DialogSettings _dialogSettings;
    [SerializeField] private RectTransform _placeForDialogues;

    [Header("Player")]
    [SerializeField] private PlayerSettings _playerSettings;


    private IEnumerator Start()
    {
        if (!CanStart()) yield break;
        Debug.Log("Start");
        SetFrameRate();

        InitSimulationSystem();
        InitUISystem();
        InitTipSystem();
        InitDialogSystem();

        InitPlayerController();



        float time = 2f;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("End");

        InitPlayerController();

        SimulationSystem.Begin();
        UISystem.Begin();
    }
    private bool CanStart()
    {
        return _simulationSettings
            && _gameSettings
            && _uiSettings
            && _tipSettings && _tipsPlace
            && _dialogSettings && _placeForDialogues
            && _playerSettings;
    }
    private void SetFrameRate()
    {
        QualitySettings.vSyncCount = _gameSettings.GameVSyncCount;
        Application.targetFrameRate = _gameSettings.GameTargetFrameRate;
    }

    #region INIT
    private void InitSimulationSystem()
    {
        SimulationSystem simSys = Instantiate(_simulationSettings.PrefabSimulationSystem);
        simSys.Init(
            _simulationSettings.SimulationStartState,
            _simulationSettings.SimulationMaxSpeed,
            _simulationSettings.SimulationMinSpeed,
            _simulationSettings.SimulationSpeedStep,
            simSys
        );
    }
    private void InitUISystem()
    {
        UISystem.Init(_uiSettings.UiPath, _uiSettings.UiSeparator, _uiSettings.UiEndLine);
    }
    private void InitTipSystem()
    {
        TipSystem.Init(_tipSettings.TipPrefab, _tipSettings.TipsMaxNumber, _tipsPlace);
    }
    private void InitDialogSystem()
    {
        DialogSystem.Init(_dialogSettings.PrefabDialogFull, _placeForDialogues, _dialogSettings.DefaultKey, _dialogSettings.DefaultPath, _dialogSettings.MinHeight, _dialogSettings.MaxHeight, _dialogSettings.AdditionalHeight);
    }
    private void InitPlayerController()
    {
        PlayerController playerController = new();
        playerController.Init(_playerSettings.PrefabPlayer, _playerSettings.BaseSpeed);
    }
    #endregion
}