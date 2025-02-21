using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO - сделать переменну€ частоту кадров, файл 'settings.ini'?
// TODO - сделать раздел "настройки"
// TODO - time scale это что?

public class SimulationSystem : MonoBehaviour
{
    [Header("Simulation's settings")]
    [SerializeField] private int _targetFramerate = 24;
    [SerializeField] private SimulationStates _simulationState = SimulationStates.Pause;

    [Header("Speed constants")]
    [SerializeField] private float MAX_SPEED = 4f;
    [SerializeField] private float MIN_SPEED = 0f;
    [SerializeField] private float SPEED_CHANGE_STEP = .25f;

    public static SimulationSystem Instance { get; private set; }
    public float CurrentSpeed { get; private set; } = 1f;
    public Action<float> TickPassed;
    public Action<bool> Played;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFramerate;

        if (!SetInstance()) return;
    }

    private void Start()
    {
        SetState(_simulationState);
    }

    private void Update()
    {
        if (IsPlayed)
        {
            TickPassed?.Invoke(Time.deltaTime * CurrentSpeed);
        }
        else if (IsPaused)
        {

        }
        else if (IsStopped)
        {
            // перезапустить сцену
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SetSpeed(SimulationSpeedStates state, float speed = 0)
    {
        switch (state)
        {
            case SimulationSpeedStates.Set:
                CurrentSpeed = speed;
                break;

            case SimulationSpeedStates.Increase:
                CurrentSpeed += SPEED_CHANGE_STEP;
                break;

            case SimulationSpeedStates.Decrease:
                CurrentSpeed -= SPEED_CHANGE_STEP;
                break;

            default:
                SimulationUtilities.DisplayWarning("Unknown state");
                break;
        }
        if (CurrentSpeed > MAX_SPEED) CurrentSpeed = MAX_SPEED;
        else if (CurrentSpeed < MIN_SPEED) CurrentSpeed = MIN_SPEED;
    }
    public void SetState(SimulationStates state)
    {
        switch (state)
        {
            case SimulationStates.Play:
                //Time.timeScale = 1;
                Played?.Invoke(true);

                break;

            case SimulationStates.Pause:
                //Time.timeScale = 0;
                Played?.Invoke(false);

                break;

            case SimulationStates.Stop:

                break;

            default:
                SimulationUtilities.DisplayWarning("Unknown state");
                _simulationState = SimulationStates.Pause;
                return;
        }
        _simulationState = state;
    }
    public void ReversePlayPause()
    {
        if (IsPlayed) SetState(SimulationStates.Pause);
        else SetState(SimulationStates.Play);
    }

    public bool IsPlayed
    {
        get => _simulationState == SimulationStates.Play;
    }
    public bool IsPaused
    {
        get => _simulationState == SimulationStates.Pause;
    }
    public bool IsStopped
    {
        get => _simulationState == SimulationStates.Stop;
    }

    private bool SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }
}

//#if ENABLE_INPUT_SYSTEM && (UNITY_IOS || UNITY_ANDROID)
//using UnityEngine.InputSystem;
//#endif