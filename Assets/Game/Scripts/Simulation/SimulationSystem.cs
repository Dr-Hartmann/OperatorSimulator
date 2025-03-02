using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using SimulationCore;

// TODO - сделать переменнуя частоту кадров, файл 'settings.ini'?
// TODO - сделать раздел "настройки"
// TODO - time scale это что?

namespace Simulation
{
    public class SimulationSystem : MonoBehaviour/*, ISimulationSystem*/
    {
        [Header("Simulation settings")]
        [SerializeField] private SimulationStates _simulationState = SimulationStates.Pause;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _minSpeed = -4f;
        [SerializeField] private float _speedStep = .25f;

        public static SimulationSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindAnyObjectByType<SimulationSystem>();
                    if (_instance != null)
                        DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        public static Action<float> EventTickPassed
        {
            get => Instance?.TickPassed;
            set
            {
                if (Instance)
                {
                    Instance.TickPassed += value;
                }
            }
        }
        public static Action<bool> EventPlayed
        {
            get => Instance?.Played;
            set
            {
                if (Instance)
                {
                    Instance.Played += value;
                }
            }
        }

        public float CurrentSpeed { get; private set; } = 1f;
        public bool IsPlayed => _simulationState == SimulationStates.Play;
        public bool IsPaused => _simulationState == SimulationStates.Pause;
        public bool IsStopped => _simulationState == SimulationStates.Stop;
        public void SetState(SimulationStates state)
        {
            switch (state)
            {
                case SimulationStates.Play:
                    //Time.timeScale = 1;
                    EventPlayed?.Invoke(true);

                    break;

                case SimulationStates.Pause:
                    //Time.timeScale = 0;
                    EventPlayed?.Invoke(false);

                    break;

                case SimulationStates.Stop:

                    break;

                default:
                    GameUtilities.DisplayWarning("Unknown state");
                    _simulationState = SimulationStates.Pause;
                    EventPlayed?.Invoke(false);
                    return;
            }
            _simulationState = state;
        }
        /// <summary>
        /// speed - абсолютный плоказатель скорости или множитель при инкременте и дикременте
        /// </summary>
        public void SetSpeed(SimulationSpeedStates state, float speed = 1)
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
                        GameUtilities.DisplayWarning("Unknown state");
                        break;
                }
                if (CurrentSpeed > _maxSpeed) CurrentSpeed = _maxSpeed;
                else if (CurrentSpeed < _minSpeed) CurrentSpeed = _minSpeed;
            }
        }
        public void ReversePlayPause()
        {
            if (IsPlayed) SetState(SimulationStates.Pause);
            else SetState(SimulationStates.Play);
        }


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (this != (UnityEngine.Object)_instance)
            {
                Destroy(this.gameObject);
            }
        }
        private void Start()
        {
            SetState(_simulationState);
        }
        private void Update()
        {
            if (IsPlayed)
            {
                EventTickPassed?.Invoke(Time.deltaTime * CurrentSpeed);
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
            EventPlayed = null;
            EventTickPassed = null;
        }
        private Action<float> TickPassed { get; set; }
        private Action<bool> Played { get; set; }
        private static SimulationSystem _instance;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoadMethod()
        {
            _instance = null;
        }
    }
}