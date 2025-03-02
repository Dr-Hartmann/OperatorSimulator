using UnityEngine;
using Simulation;

public class PanelStop : MonoBehaviour
{
    private void OnEnable()
    {
        SimulationSystem.EventPlayed -= SetActive;
        SimulationSystem.EventPlayed += SetActive;
    }
    private void OnDestroy()
    {
        SimulationSystem.EventPlayed -= SetActive;
    }
    private void SetActive(bool _isPlayed)
    {
        this.gameObject.SetActive(!_isPlayed); 
    }
}