using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO - сделать переменну€ частоту кадров, файл 'settings.ini'?
// TODO - сделать раздел "настройки"
// TODO - time scale это что?

/// <summary>
/// ядро симул€ции
/// </summary>
public class SimulationSystem : MonoBehaviour
{
    [Header("Simulation settings")]
    [SerializeField] private SimulationStates _simulationState = SimulationStates.Pause;
    [SerializeField] private float _maxSpeed = 6f;
    [SerializeField] private float _minSpeed = 0f;
    [SerializeField] private float _step = .25f;

    public static SimulationSystem Instance { get; private set; }
    public static Action<float> TickPassed;
    public static Action<bool> Played;

    public static float CurrentSpeed { get; private set; } = 1f;
    

    /// <summary>
    /// speed - абсолютный плоказатель скорости или множитель при инкременте и дикременте
    /// </summary>
    public void SetSpeed(SimulationSpeedStates state, float speed = 1)
    {
        switch (state)
        {
            case SimulationSpeedStates.Set:
                CurrentSpeed = speed;
                break;

            case SimulationSpeedStates.Increase:
                CurrentSpeed += _step * speed;
                break;

            case SimulationSpeedStates.Decrease:
                CurrentSpeed -= _step * speed;
                break;

            default:
                SimulationUtilities.DisplayWarning("Unknown state");
                break;
        }
        if (CurrentSpeed > _maxSpeed) CurrentSpeed = _maxSpeed;
        else if (CurrentSpeed < _minSpeed) CurrentSpeed = _minSpeed;
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
                Played?.Invoke(false);
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


    private void Awake()
    {
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void OnDestroy()
    {
        Played = null;
        TickPassed = null;
    }
    private bool SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }


    [RuntimeInitializeOnLoadMethod]
    public static void InitializeOnLoadMethod()
    {
        Played = null;
        TickPassed = null;
    }
}