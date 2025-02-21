using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SpeedValue : MonoBehaviour
{
    private TextMeshProUGUI _thisText;

    private void Awake()
    {
        _thisText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _thisText.SetText(SimulationSystem.Instance.CurrentSpeed.ToString());
    }
}
