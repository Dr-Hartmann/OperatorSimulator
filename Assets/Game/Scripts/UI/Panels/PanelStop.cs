using UnityEngine;

public class PanelStop : MonoBehaviour
{
    private void Start()
    {
        SimulationSystem.Played += SetActive;
    }
    private void OnDestroy()
    {
        SimulationSystem.Played -= SetActive;
    }
    private void SetActive(bool _isPlayed)
    {
        this.gameObject.SetActive(!_isPlayed); 
    }
}
