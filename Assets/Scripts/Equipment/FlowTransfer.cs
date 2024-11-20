using UnityEngine;

public class FlowTransfer : MonoBehaviour
{
    [SerializeField] private Source _source;
    [SerializeField] private Tank _destinationTank;
    [SerializeField] private Valve _valve;

    private void Update()
    {
        float sourceFlow = _source.GetFlowRate();
        float flowThroughValve = _valve.GetFlowRate(sourceFlow);
        _destinationTank.Fill(flowThroughValve * Time.deltaTime);
    }
}
