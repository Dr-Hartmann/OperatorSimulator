using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Simulation : MonoBehaviour
{
    [Header("Simulation's settings")]
    [SerializeField] private int _targetFramerate = 30;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private SimulationSpeedController _speedController;

    public Action<float> Tick;
    private SimulationStateEnum _state;

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFramerate;
        //QualitySettings.vSyncCount = 1;
        //if (Application.targetFrameRate > _targetFramerate - 1)
        //{
        //    QualitySettings.vSyncCount = 0;
        //    Application.targetFrameRate = _targetFramerate;
        //}
    }

    public void Start()
    {
        this.gameObject.SetActive(true);
        SimulationStart();
    }

    public void Update()
    {
        // вызов делегата и передача данных сколько времени прошло с момента последнего кадра
        if (IsStarted())
        {
            Tick?.Invoke(Time.deltaTime * _speedController.Speed);
            _speedController.SetSpeed(_speedController.Speed + 0.0001f * Application.targetFrameRate / 60);
            _speedText.SetText($"{LocalizationSystem.instance.GetLocalizedText("UI.Speed")} {_speedController.SpeedToUI}");
        }
    }

    public void SimulationStart() => _state = SimulationStateEnum.Started;
    public bool IsStarted() => _state == SimulationStateEnum.Started;
    public void SimulationStop() => _state = SimulationStateEnum.Stopped;
    public bool IsStopped() => _state == SimulationStateEnum.Stopped;
    public void SimulationPause() => _state = SimulationStateEnum.Paused;
    public bool IsPaused() => _state == SimulationStateEnum.Paused;
}