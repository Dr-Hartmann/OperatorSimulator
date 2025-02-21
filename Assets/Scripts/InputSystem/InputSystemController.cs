using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _playPause;
    private InputAction _speedMore;
    private InputAction _speedLess;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        _playPause = _playerInput.actions["PlayPause"];
        _speedMore = _playerInput.actions["SpeedMore"];
        _speedLess = _playerInput.actions["SpeedLess"];
        _playPause.performed += PlayPause;
        _speedMore.performed += SpeedMore;
        _speedLess.performed += SpeedLess;
        
    }

    private void PlayPause(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance.ReversePlayPause();
    }
    private void SpeedMore(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance.SetSpeed(SimulationSpeedStates.Increase);
    }
    private void SpeedLess(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance.SetSpeed(SimulationSpeedStates.Decrease);
    }

    private void OnDestroy()
    {
        _playPause.performed -= PlayPause;
        _speedMore.performed -= SpeedMore;
        _speedLess.performed -= SpeedLess;
    }

}
