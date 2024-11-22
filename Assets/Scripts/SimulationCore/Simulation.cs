using System;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public Action<float> Tick;
    private StateSimulation _state = StateSimulation.Started;
    public SimulationSpeedController _speedController;

    public void Start()
    {

    }

    public void Update()
    {
        Tick?.Invoke(Time.deltaTime * _speedController.Speed);
    }

    public void StartSimulation() => _state = StateSimulation.Started;
    public void StopSimulation() => _state = StateSimulation.Stopped;
    public void PauseSimulation() => _state = StateSimulation.Paused;
}