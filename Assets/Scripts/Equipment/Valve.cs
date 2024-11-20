using UnityEngine;

public class Valve : MonoBehaviour
{
    [SerializeField] private float _maxFlowRate; // ������������ ����� ����� ������
    [SerializeField] private bool _isOpen; // ��������� �������

    public float GetFlowRate(float inputFlowRate)
    {
        if (!_isOpen) return 0f; // ���� ������ ������, ����� ����� 0
        return Mathf.Clamp(inputFlowRate, 0, _maxFlowRate);
    }

    public void SetOpenState(bool state)
    {
        _isOpen = state;
    }
}
