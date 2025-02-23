using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemController : MonoBehaviour
{
    [SerializeField] private GameObject _prefabDialog;

    private InputSystemActions _playerInput;
    private RectTransform _canvas;

    [SerializeField] private Player player;
    

    private void PlayPause(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance?.ReversePlayPause();
    }
    private void SpeedMore(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance?.SetSpeed(SimulationSpeedStates.Increase);
    }
    private void SpeedLess(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance?.SetSpeed(SimulationSpeedStates.Decrease);
    }
    private void StartDialog(InputAction.CallbackContext context)
    { 
        if (GameObject.FindGameObjectWithTag(_prefabDialog.tag)) return;
        GameObject obj = Instantiate(_prefabDialog, _canvas);
        Dialog txt = obj.GetComponentInChildren<Dialog>();
        txt.Path = "Dialog/greeting";
        txt.StartDialog("test");
    }
    private void Restart(InputAction.CallbackContext context)
    {
        SimulationSystem.Instance?.SetState(SimulationStates.Stop);
    }

    private void E(InputAction.CallbackContext context)
    {
        player.SetBehaviorIdle();
    }
    private void C(InputAction.CallbackContext context)
    {
        player.SetBehaviorActive();
    }
    private void Space(InputAction.CallbackContext context)
    {
        player.SetBehaviorAggressive();
    }

    private void SubscribeAll()
    {
        _playerInput.UI.PlayPause.performed += PlayPause;
        _playerInput.UI.SpeedMore.performed += SpeedMore;
        _playerInput.UI.SpeedLess.performed += SpeedLess;
        _playerInput.UI.Restart.performed += Restart;

        _playerInput.Player.Interact.performed += StartDialog;

        _playerInput.Player.Interact.performed += E;
        _playerInput.Player.Crouch.performed += C;
        _playerInput.Player.Jump.performed += Space;
    }
    private void UnsubscribeAll()
    {
        _playerInput.UI.PlayPause.performed -= PlayPause;
        _playerInput.UI.SpeedMore.performed -= SpeedMore;
        _playerInput.UI.SpeedLess.performed -= SpeedLess;
        _playerInput.UI.Restart.performed -= Restart;

        _playerInput.Player.Interact.performed -= StartDialog;

        _playerInput.Player.Interact.performed -= E;
        _playerInput.Player.Crouch.performed -= C;
        _playerInput.Player.Jump.performed -= Space;
    }

    private void Awake()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponentInParent<RectTransform>();
        _playerInput = new();
    }
    
    private void OnEnable()
    {
        _playerInput.UI.Enable();
        _playerInput.Player.Enable();
        SubscribeAll();
    }
    private void OnDisable()
    {
        _playerInput.UI.Disable();
        _playerInput.Player.Disable();
        UnsubscribeAll();
    }
    private void OnDestroy()
    {
        OnDisable();
    }
}