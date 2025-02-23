using UnityEngine;

[CreateAssetMenu(fileName = "SubstanceData", menuName = "Scriptable Objects/SubstanceData")]
public class SubstanceData : ScriptableObject
{
    [SerializeField] private string _name; // �������� ��������
    [SerializeField] private float _density; // ��������� ��������
    [SerializeField] private float _viscosity; // �������� ��������

    public string Name => _name;
    public float Density => _density;
    public float Viscosity => _viscosity;
}
