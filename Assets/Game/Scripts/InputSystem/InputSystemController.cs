using UnityEngine;
using PlayerSpace;
using Simulation;

public class InputSystemController : MonoBehaviour
{
    [SerializeField] private Player _player;
    

    private InputSystem_Actions _inputActions;
    private InputSystem_Actions.UIActions _uiAct;
    private InputSystem_Actions.PlayerActions _playerAct;
    

    //private bool _canCancel = true;
    private bool _isChanged = false;
    private bool _move = false;
    private bool _attack = false;
    

    private /*async*/ void Update()
    {
        if (!_isChanged) return;
        //await Task.Run(() => { while (!_canCancel) { }; });

        if (_attack) _player.SetBehaviorAttacking();
        else if (_move) _player.SetBehaviorMoving();
        else _player.SetBehaviorIdle();
            
        _isChanged = false;
    }

    // для аниматора
    public void SetCanCancel()
    {
        //_canCancel = true;
    }
    public void SetCannotCancel()
    {
        //_canCancel = false;
    }
    // /для аниматора


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
    }
    private void OnEnable()
    {
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

    private void StartDialog()
    {
        
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
    private void AttackCanceled()
    {
        _attack = false;
        _isChanged = true;
    }

    private void PlayPause()
    {
        SimulationSystem.ReversePlayPause();
        _player.UpdateAnimatorSpeed();
    }
    private void SpeedMore()
    {
        SimulationSystem.SetSpeed(SimulationSpeedStates.Increase);
        _player.UpdateAnimatorSpeed();
    }
    private void SpeedLess()
    {
        SimulationSystem.SetSpeed(SimulationSpeedStates.Decrease);
        _player.UpdateAnimatorSpeed();
    }
    private void Restart()
    {
        SimulationSystem.SetState(SimulationStates.Stop);
    }
}