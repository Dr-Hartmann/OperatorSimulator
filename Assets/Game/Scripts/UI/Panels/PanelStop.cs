using UnityEngine;
using SimulationCore;

public class PanelStop : MonoBehaviour
{
    private void OnEnable()
    {
        ISimulationSystem.Played -= SetActive;
        ISimulationSystem.Played += SetActive;
    }
    private void OnDestroy()
    {
        ISimulationSystem.Played -= SetActive;
    }
    private void SetActive(bool _isPlayed)
    {
        this.gameObject.SetActive(!_isPlayed); 
    }
}