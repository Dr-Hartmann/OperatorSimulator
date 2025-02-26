using UnityEngine;
using UnityEngine.SceneManagement;
using SimulationCore;

// TODO - сделать переменну€ частоту кадров, файл 'settings.ini'?
// TODO - сделать раздел "настройки"
// TODO - time scale это что?

namespace Simulation
{
    public class SimulationSystem : MonoBehaviour, ISimulationSystem
    {
        [Header("Simulation settings")]
        [SerializeField] private SimulationStates _simulationState = SimulationStates.Pause;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _minSpeed = -4f;
        [SerializeField] private float _speedStep = .25f;
        public static ISimulationSystem Instance { get; private set; }


        private void Awake()
        {
            if (!SetInstance()) return;
            ISimulationSystem.State = _simulationState;
            ISimulationSystem.SpeedStep = _speedStep;
            ISimulationSystem.MaxSpeed = _maxSpeed;
            ISimulationSystem.MinSpeed = _minSpeed;
        }
        private void Start()
        {
            ISimulationSystem.SetState(_simulationState);
        }
        private void Update()
        {
            if (ISimulationSystem.IsPlayed)
            {
                ISimulationSystem.TickPassed?.Invoke(Time.deltaTime * ISimulationSystem.CurrentSpeed);
            }
            else if (ISimulationSystem.IsPaused)
            {

            }
            else if (ISimulationSystem.IsStopped)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        private void OnDestroy()
        {
            ISimulationSystem.InitializeOnLoadMethod();
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
}