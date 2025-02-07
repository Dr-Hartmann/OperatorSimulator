using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class ControlButtonSpeedLess : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        SimulationSystem.instance.SpeedDecrease();
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}