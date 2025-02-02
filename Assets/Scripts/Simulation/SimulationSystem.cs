using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimulationSystem : MonoBehaviour
{
    [Header("Simulation's settings")]
    [SerializeField] private int _targetFramerate = 24;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private SimulationSpeedController _speedController;

    // TODO - ���������� ������� � ������ Update ������ ��������
    public Action<float> Tick;
    private SimulationStateEnum _state;

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFramerate;
        // TODO - ���������� ������� ������, ���� 'settings.ini'?
        //QualitySettings.vSyncCount = 1;
        //if (Application.targetFrameRate > _targetFramerate - 1)
        //{
        //    QualitySettings.vSyncCount = 0;
        //    Application.targetFrameRate = _targetFramerate;
        //}
        this.gameObject.SetActive(true);
    }

    public void Start()
    {
        SimulationStart();
    }

    public void Update()
    {
        // ����� �������� � �������� ������ ������� ������� ������ � ������� ���������� �����
        if (IsStarted())
        {
            Tick?.Invoke(Time.deltaTime * _speedController.Speed);
            _speedController.SetSpeed(_speedController.Speed + 0.0001f * Application.targetFrameRate);
            // TODO - ����������
            //_speedText.SetText($"{LocalizationSystem.instance.GetLocalizedText("UI.Speed")} {_speedController.Speed}");
        }
    }

    public void SimulationStart() => _state = SimulationStateEnum.Started;
    public bool IsStarted() => _state == SimulationStateEnum.Started;
    
    public void SimulationPause() => _state = SimulationStateEnum.Paused;
    public bool IsPaused() => _state == SimulationStateEnum.Paused;

    public void SimulationStop() => _state = SimulationStateEnum.Stopped;
    public bool IsStopped() => _state == SimulationStateEnum.Stopped;
}