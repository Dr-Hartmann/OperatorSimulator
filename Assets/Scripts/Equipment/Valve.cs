using UnityEngine;

public class Valve : MonoBehaviour
{
    [SerializeField] private float _maxFlowRate; // Максимальный поток через клапан
    [SerializeField] private bool _isOpen; // Состояние клапана

    public float GetFlowRate(float inputFlowRate)
    {
        if (!_isOpen) return 0f; // Если клапан закрыт, поток равен 0
        return Mathf.Clamp(inputFlowRate, 0, _maxFlowRate);
    }

    public void SetOpenState(bool state)
    {
        _isOpen = state;
    }
}
