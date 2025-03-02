using System;

namespace SimulationCore
{
    public interface ISimulationSystem
    {
        Action<float> EventTickPassed { get; set; }
        Action<bool> EventPlayed { get; set; }
        float CurrentSpeed { get; }
    }
}