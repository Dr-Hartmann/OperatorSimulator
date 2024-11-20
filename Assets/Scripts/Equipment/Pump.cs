using UnityEngine;

public class Pump : MonoBehaviour
{
    [SerializeField] private AnimationCurve _pressureCurve; // ������ ����������� �������� �� �������
    [SerializeField] private float _maxFlowRate; // ������������ �����

    private float _currentPressure;
    private float _elapsedTime;

    private void Start()
    {
        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        // ���������� �������� �� ������ AnimationCurve
        _currentPressure = _pressureCurve.Evaluate(_elapsedTime);
    }

    public float GetFlowRate()
    {
        // ������ ������ �� ������ ��������
        return Mathf.Clamp(_currentPressure / _maxFlowRate, 0f, _maxFlowRate);
    }
}
