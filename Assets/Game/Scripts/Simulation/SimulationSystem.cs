using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// TODO - сделать переменнуя частоту кадров, файл 'settings.ini'?
// TODO - сделать раздел "настройки"
// TODO - time scale это что?

namespace Simulation
{
    /// <summary>
    /// Контролирует состояние симуляции  и сообщает её скорость.
    /// </summary>
    public class SimulationSystem : MonoBehaviour
    {
        [SerializeField] SimulationSettings _simulationSettings;

        private Action<float> TickPassed;
        private Action<bool> Played;

        public static void SubEventTickPassed(Action<float> action)
        {
            Instance.TickPassed += action;
        }
        public static void UnsubEventTickPassed(Action<float> action)
        {
            if (_instance) _instance.TickPassed -= action;
        }
        public static void SubEventEventPlayed(Action<bool> action)
        {
            Instance.Played += action;
        }
        public static void UnsubEventPlayed(Action<bool> action)
        {
            if (_instance) _instance.Played -= action;
        }
        public static float CurrentSpeed { get; private set; } = 1f;
        public static bool IsPlayed => _simulationState == SimulationStates.Play;
        public static bool IsPaused => _simulationState == SimulationStates.Pause;
        public static bool IsStopped => _simulationState == SimulationStates.Stop;
        public static void SetState(SimulationStates state)
        {
            switch (state)
            {
                case SimulationStates.Play:
                    //Time.timeScale = 1;
                    Instance.Played?.Invoke(true);

                    break;

                case SimulationStates.Pause:
                    //Time.timeScale = 0;
                    Instance.Played?.Invoke(false);

                    break;

                case SimulationStates.Stop:

                    break;

                default:
                    GameUtilities.Debug.GameUtilities.DisplayWarning("Unknown state");
                    _simulationState = SimulationStates.Pause;
                    Instance.Played?.Invoke(false);
                    return;
            }
            _simulationState = state;
        }
        /// <summary>speed - абсолютный плоказатель скорости или множитель при инкременте и декременте.</summary>
        public static void SetSpeed(SimulationSpeedStates state, float speed = 1)
        {
            {
                switch (state)
                {
                    case SimulationSpeedStates.Set:
                        CurrentSpeed = speed;
                        break;

                    case SimulationSpeedStates.Increase:
                        CurrentSpeed += _speedStep * speed;
                        break;

                    case SimulationSpeedStates.Decrease:
                        CurrentSpeed -= _speedStep * speed;
                        break;

                    default:
                        GameUtilities.Debug.GameUtilities.DisplayWarning("Unknown state");
                        break;
                }
                if (CurrentSpeed > _maxSpeed) CurrentSpeed = _maxSpeed;
                else if (CurrentSpeed < _minSpeed) CurrentSpeed = _minSpeed;
            }
        }
        public static void ReversePlayPause()
        {
            if (IsPlayed) SetState(SimulationStates.Pause);
            else SetState(SimulationStates.Play);
        }


        private void Awake()
        {
            if (!_simulationSettings) return;
            _simulationState = _simulationSettings.SimulationStartState;
            _maxSpeed = _simulationSettings.SimulationMaxSpeed;
            _minSpeed = _simulationSettings.SimulationMinSpeed;
            _speedStep = _simulationSettings.SimulationSpeedStep;
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
        
        private static SimulationSystem Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindAnyObjectByType<SimulationSystem>();
                    if (!_instance) return null;
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private static SimulationSystem _instance;
        private static SimulationStates _simulationState;
        private static float _maxSpeed;
        private static float _minSpeed;
        private static float _speedStep;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoadMethod()
        {
            _instance = null;
            _simulationState = SimulationStates.Play;
            _maxSpeed = 0;
            _minSpeed = 0;
            _speedStep = 0;
        }
    }
}