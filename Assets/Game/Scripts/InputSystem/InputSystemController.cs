using UnityEngine;
using PlayerSpace;
using Simulation;
using System.Threading.Tasks;

public class InputSystemController : MonoBehaviour
{
    [SerializeField] private GameObject _prefabDialog;
    [SerializeField] private Player _player;
    [SerializeField] private RectTransform _canvas;

    private InputSystem_Actions _inputActions;
    private InputSystem_Actions.UIActions _uiAct;
    private InputSystem_Actions.PlayerActions _playerAct;


    
    private bool _canCancel = true;

    private bool _move = false;
    private bool _attack = false;
    private bool _isChanged = false;

    private void Update()
    {
        if (!_isChanged) return;

        if (_attack) _player.SetBehaviorAttacking();
        else if (_move) _player.SetBehaviorMoving();
        else _player.SetBehaviorIdle();

        _isChanged = false;
    }

    // для аниматора
    public void SetCanCancel()
    {
        _canCancel = true;
    }
    public void SetCannotCancel()
    {
        _canCancel = false;
    }
    //


    public void SubscribeAll()
    {
        UnsubscribeAll();
        SubscribePlayer();
        SubscribeUI();
    }
    public void UnsubscribeAll()
    {
        UnsubscribePlayer();
        UnsubscribeUI();
    }
    public void SubscribeUI()
    {
        _uiAct.PlayPause.performed += context => PlayPause();
        _uiAct.SpeedMore.performed += context => SpeedMore();
        _uiAct.SpeedLess.performed += context => SpeedLess();
        _uiAct.Restart.performed += context => Restart();
    }
    public void UnsubscribeUI()
    {
        _uiAct.PlayPause.performed -= context => PlayPause();
        _uiAct.SpeedMore.performed -= context => SpeedMore();
        _uiAct.SpeedLess.performed -= context => SpeedLess();
        _uiAct.Restart.performed -= context => Restart();
    }
    public void SubscribePlayer()
    {
        _playerAct.Interact.performed += context => StartDialog();

        _playerAct.Move.started += context => MoveStarted();
        _playerAct.Move.performed += context => MovePerformed(context.ReadValue<Vector2>());
        _playerAct.Move.canceled += context => MoveCanceled();

        _playerAct.Attack.started += context => AttackStarted();
        _playerAct.Attack.canceled += context => AttackCanceled();
    }
    public void UnsubscribePlayer()
    {
        _playerAct.Interact.performed -= context => StartDialog();

        _playerAct.Move.started -= context => MoveStarted();
        _playerAct.Move.performed -= context => MovePerformed(context.ReadValue<Vector2>());
        _playerAct.Move.canceled -= context => MoveCanceled();

        _playerAct.Attack.started -= context => AttackStarted();
        _playerAct.Attack.canceled -= context => AttackCanceled();
    }


    private void Awake()
    {
        _inputActions = new();
        _uiAct = _inputActions.UI;
        _playerAct = _inputActions.Player;

        SubscribeAll();
        _uiAct.Enable();
        _playerAct.Enable();
    }
    private void OnDestroy()
    {
        UnsubscribeAll();
        _uiAct.Disable();
        _playerAct.Disable();
    }

    private async Task DelayCancel()
    {
        await Task.Run(() => { while (!_canCancel) { }; });
    }

    private void StartDialog()
    {
        if (GameObject.FindGameObjectWithTag(_prefabDialog.tag)) return;
        GameObject obj = Instantiate(_prefabDialog, _canvas);
        Dialog txt = obj.GetComponentInChildren<Dialog>();
        txt.Path = "Dialog/greeting";
        txt.StartDialog("test");
    }

    private void MoveStarted()
    {
        _move = true;
        _isChanged = true;
    }
    private void MovePerformed(Vector2 vector)
    {
        _player.MoveVector = vector;
    }
    private void MoveCanceled()
    {
        _move = false;
        _isChanged = true;
    }

    private void AttackStarted()
    {
        _attack = true;
        _isChanged = true;
    }
    private async void AttackCanceled()
    {
        await DelayCancel();
        _attack = false;
        _isChanged = true;
    }

    private void PlayPause()
    {
        SimulationSystem.Instance.ReversePlayPause();
        _player.UpdateAnimatorSpeed();
    }
    private void SpeedMore()
    {
        SimulationSystem.Instance.SetSpeed(SimulationSpeedStates.Increase);
        _player.UpdateAnimatorSpeed();
    }
    private void SpeedLess()
    {
        SimulationSystem.Instance.SetSpeed(SimulationSpeedStates.Decrease);
        _player.UpdateAnimatorSpeed();
    }
    private void Restart()
    {
        SimulationSystem.Instance.SetState(SimulationStates.Stop);
    }
}