using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpeedLess : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        SimulationSystem.Instance.SetSpeed(SimulationSpeedStates.Decrease);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}