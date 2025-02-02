//#if ENABLE_INPUT_SYSTEM && (UNITY_IOS || UNITY_ANDROID)
//using UnityEngine.InputSystem;
//#endif

using UnityEngine;

public class SimulationSpeedController : MonoBehaviour
{
    [Header("Speed constants")]
    [SerializeField] private float MAX_SPEED = 2f;
    [SerializeField] private float MIN_SPEED = 0f;
    [SerializeField] private float SPEED_CHANGE_STEP = .25f;

    public float Speed { get; private set; } = 1f;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    public void SetSpeed(float speed)
    {
        if (speed < MIN_SPEED) Speed = MIN_SPEED;
        else if (speed > MAX_SPEED) Speed = MAX_SPEED;
        else Speed = speed;
    }

    public void IncreaseSpeed()
    {
        Speed += SPEED_CHANGE_STEP;
        if (Speed > MAX_SPEED) Speed = MAX_SPEED;
    }

    public void DecreaseSpeed()
    {
        Speed -= SPEED_CHANGE_STEP;
        if (Speed < MIN_SPEED) Speed = MIN_SPEED;
    }
}