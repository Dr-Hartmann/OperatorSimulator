using UnityEngine;

public class Source : MonoBehaviour
{
    [SerializeField] private float _flowRate; // ���������� ����� ���������

    public float GetFlowRate()
    {
        return _flowRate;
    }
}
