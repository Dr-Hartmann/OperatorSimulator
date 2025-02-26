using System;
using UnityEngine;

namespace SimulationCore
{
    /// <summary>
    /// Ядро симуляции
    /// </summary>
    public interface ISimulationSystem
    {
        public static Action<float> TickPassed;
        public static Action<bool> Played;
        static float CurrentSpeed { get; private set; } = 1f;
        /// <summary>
        /// speed - абсолютный плоказатель скорости или множитель при инкременте и дикременте
        /// </summary>
        static void SetSpeed(SimulationSpeedStates state, float speed = 1)
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
                    GameUtilities.DisplayWarning("Unknown state");
                    break;
            }
            if (CurrentSpeed > MaxSpeed) CurrentSpeed = MaxSpeed;
            else if (CurrentSpeed < MinSpeed) CurrentSpeed = MinSpeed;
        }
        static void SetState(SimulationStates state)
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
                    GameUtilities.DisplayWarning("Unknown state");
                    State = SimulationStates.Pause;
                    Played?.Invoke(false);
                    return;
            }
            State = state;
        }
        static void ReversePlayPause()
        {
            if (IsPlayed) SetState(SimulationStates.Pause);
            else SetState(SimulationStates.Play);
        }
        static bool IsPlayed
        {
            get => State == SimulationStates.Play;
        }
        static bool IsPaused
        {
            get => State == SimulationStates.Pause;
        }
        static bool IsStopped
        {
            get => State == SimulationStates.Stop;
        }
        static SimulationStates State { get; set; }
        static float SpeedStep { get; set; }
        static float MaxSpeed { get; set; }
        static float MinSpeed { get; set; }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeOnLoadMethod()
        {
            Played = null;
            TickPassed = null;
        }
    }
}