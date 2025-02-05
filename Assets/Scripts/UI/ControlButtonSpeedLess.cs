using UnityEngine;
using UnityEngine.UI;

internal class ControlButtonSpeedLess : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        SimulationSystem.instance.SpeedIncrease();
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}