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
        #region PUBLIC
        public static void SubEventTickPassed(Action<float> action)
        {
            TickPassed += action;
        }
        public static void UnsubEventTickPassed(Action<float> action)
        {
            TickPassed -= action;
        }
        public static void SubEventEventPlayed(Action<bool> action)
        {
            Played += action;
        }
        public static void UnsubEventPlayed(Action<bool> action)
        {
            Played -= action;
        }
        public static void SetState(SimulationStates state)
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
                    GameUtilities.Debug.GameUtilities.DisplayWarning("Unknown state");
                    SimulationState = SimulationStates.Pause;
                    Played?.Invoke(false);
                    return;
            }

            SimulationState = state;
        }
        /// <summary>speed - абсолютный плоказатель скорости или множитель при инкременте и декременте.</summary>
        public static void SetSpeed(SimulationSpeedStates state, float speed = 1)
        {
            switch (state)
            {
                case SimulationSpeedStates.Set:
                    CurrentSpeed = speed;
                    break;

                case SimulationSpeedStates.Increase:
                    CurrentSpeed += SpeedStep * speed;
                    break;

                case SimulationSpeedStates.Decrease:
                    CurrentSpeed -= SpeedStep * speed;
                    break;

                default:
                    GameUtilities.Debug.GameUtilities.DisplayWarning("Unknown state");
                    break;
            }

            if (CurrentSpeed > MaxSpeed) CurrentSpeed = MaxSpeed;
            else if (CurrentSpeed < MinSpeed) CurrentSpeed = MinSpeed;
        }
        public static void ReversePlayPause()
        {
            if (IsPlayed) SetState(SimulationStates.Pause);
            else SetState(SimulationStates.Play);
        }
        public static float CurrentSpeed { get; private set; } = 1f;
        public static bool IsPlayed => SimulationState == SimulationStates.Play;
        public static bool IsPaused => SimulationState == SimulationStates.Pause;
        public static bool IsStopped => SimulationState == SimulationStates.Stop;
        #endregion

        #region CORE
        public void Init(SimulationStates startState, float maxSpeed, float minSpeed, float speedStep, SimulationSystem instance)
        {
            if (Instance != null) { Destroy(this.gameObject); return; }
            DontDestroyOnLoad(this.gameObject);
            SimulationState = startState;
            MaxSpeed = maxSpeed;
            MinSpeed = minSpeed;
            SpeedStep = speedStep;
            Instance = instance;
        }
        public static void Begin()
        {
            SetState(SimulationState);
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
            TickPassed = null;
            Played = null;
        }
        #endregion

        #region variables
        private static Action<float> TickPassed { get; set; }
        private static Action<bool> Played { get; set; }
        private static SimulationSystem Instance { get; set; }
        private static SimulationStates SimulationState { get; set; }
        private static float MaxSpeed { get; set; }
        private static float MinSpeed { get; set; }
        private static float SpeedStep { get; set; }
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoadMethod()
        {
            TickPassed = null;
            Played = null;
        }
    }
}