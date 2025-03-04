using UnityEngine;
using Simulation;

public class PanelStop : MonoBehaviour
{
    private void OnEnable()
    {
        SimulationSystem.UnsubEventPlayed(SetActive);
        SimulationSystem.SubEventEventPlayed(SetActive);
    }
    private void OnDestroy()
    {
        SimulationSystem.UnsubEventPlayed(SetActive);
    }
    private void SetActive(bool _isPlayed)
    {
        this.gameObject.SetActive(!_isPlayed); 
    }
}