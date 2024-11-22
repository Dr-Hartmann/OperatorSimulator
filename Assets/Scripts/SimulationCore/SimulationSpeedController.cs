using UnityEngine;

public class SimulationSpeedController : MonoBehaviour
{
    [SerializeField] private const int MAX_SPEED = 100;
    [SerializeField] private const int SPEED_CHANGE_STEP = 2;
    private int _speed;

    public string SpeedToUI => _speed.ToString();
    public int Speed => _speed;

    public void SetSpeed(int speed)
    {
        if (_speed == speed) return;
        if (speed < 0) _speed = 0;
        else if (speed > MAX_SPEED) _speed = MAX_SPEED;
        else _speed = speed;
    }

    public void IncreaseSpeed()
    {
        _speed += SPEED_CHANGE_STEP;
    }

    public void DecreaseSpeed()
    {
        _speed -= SPEED_CHANGE_STEP;
    }
}
