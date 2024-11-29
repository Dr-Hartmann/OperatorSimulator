//#if ENABLE_INPUT_SYSTEM && (UNITY_IOS || UNITY_ANDROID)
//using UnityEngine.InputSystem;
//#endif

//[Header("Speed")]
//[Tooltip("Current simulation speed")]
//[Range(MIN_SPEED, MAX_SPEED)]    -    не работает не с константами

using UnityEngine;

public class SimulationSpeedController : MonoBehaviour
{
    [Header("Speed constants")]
    [SerializeField] private int MAX_SPEED = 100;
    [SerializeField] private int MIN_SPEED = 0;
    [SerializeField] private int SPEED_CHANGE_STEP = 2;

    public int Speed { get; private set; }

    public string SpeedToUI => Speed.ToString();

    public void SetSpeed(int speed)
    {
        if (Speed == speed) return;
        if (speed < MIN_SPEED) Speed = MIN_SPEED;
        else if (speed > MAX_SPEED) Speed = MAX_SPEED;
        else Speed = speed;
    }

    public void IncreaseSpeed()
    {
        if (Speed == MAX_SPEED) return;
        if (Speed + SPEED_CHANGE_STEP > MAX_SPEED) Speed = MAX_SPEED;
        Speed += SPEED_CHANGE_STEP;
    }

    public void DecreaseSpeed()
    {
        if (Speed == MIN_SPEED) return;
        if (Speed - SPEED_CHANGE_STEP < MIN_SPEED) Speed = MIN_SPEED;
        Speed -= SPEED_CHANGE_STEP;
    }
}