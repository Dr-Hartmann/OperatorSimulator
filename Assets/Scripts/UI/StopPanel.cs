using UnityEngine;

public class StopPanel : MonoBehaviour
{
    private void Start()
    {
        SimulationSystem.Instance.Played += SetActive;
    }

    private void OnDestroy()
    {
        SimulationSystem.Instance.Played -= SetActive;
    }

    private void SetActive(bool _isPlayed)
    {
        this.gameObject.SetActive(!_isPlayed);
    }
}
