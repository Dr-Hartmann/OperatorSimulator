using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class SimulationSystem : MonoBehaviour
{
    [Header("Simulation's settings")]
    [SerializeField] private int _targetFramerate = 24;
    [SerializeField] public SimulationStates SimulationState { get; private set; } = SimulationStates.Paused;
    [SerializeField] private TextMeshProUGUI _speedText;

    [Header("Speed constants")]
    [SerializeField] private float MAX_SPEED = 4f;
    [SerializeField] private float MIN_SPEED = 0f;
    [SerializeField] private float SPEED_CHANGE_STEP = .25f;

    public static SimulationSystem instance { get; private set; }
    public float CurrentSpeed { get; private set; } = 1f;
    public Action<float> TickPassed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFramerate;

        // TODO - переменная частота кадров, файл 'settings.ini'?
        //QualitySettings.vSyncCount = 1;
        //if (Application.targetFrameRate > _targetFramerate - 1)
        //{
        //    QualitySettings.vSyncCount = 0;
        //    Application.targetFrameRate = _targetFramerate;
        //}

        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        SimulationStart();
    }

    private void Update()
    {
        _speedText.SetText(CurrentSpeed.ToString());// TODO - убрать отсюда

        if (IsStarted)
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

    // контроль скорости симуляции
    public void SpeedSet(float speed)
    {
        if (speed < MIN_SPEED) CurrentSpeed = MIN_SPEED;
        else if (speed > MAX_SPEED) CurrentSpeed = MAX_SPEED;
        else CurrentSpeed = speed;
    }
    public void SpeedIncrease()
    {
        CurrentSpeed += SPEED_CHANGE_STEP;
        if (CurrentSpeed > MAX_SPEED) CurrentSpeed = MAX_SPEED;
    }
    public void SpeedDecrease()
    {
        CurrentSpeed -= SPEED_CHANGE_STEP;
        if (CurrentSpeed < MIN_SPEED) CurrentSpeed = MIN_SPEED;
    }

    // контроль состояния симуляции
    public void SimulationStart() => SimulationState = SimulationStates.Started;
    public bool IsStarted => SimulationState == SimulationStates.Started;
    public void SimulationPause() => SimulationState = SimulationStates.Paused;
    public bool IsPaused => SimulationState == SimulationStates.Paused;
    public void SimulationStop() => SimulationState = SimulationStates.Stopped;
    public bool IsStopped => SimulationState == SimulationStates.Stopped;
}

//#if ENABLE_INPUT_SYSTEM && (UNITY_IOS || UNITY_ANDROID)
//using UnityEngine.InputSystem;
//#endif