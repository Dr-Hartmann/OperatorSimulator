using UnityEngine;
//TODO: ���������� �� ���������������� ECS
public class Solver : MonoBehaviour
{
    [SerializeField] private PipeSegment[] _pipeSegments; // ��� �������� ���� � �������

    public void SolveFlow()
    {
        foreach (var pipe in _pipeSegments)
        {
            pipe.Update();
        }
    }
}
