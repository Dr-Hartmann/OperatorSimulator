using UnityEngine;
using PlayerSpace;
using SimulationCore;
using System.Threading.Tasks;

[RequireComponent(typeof(Player))]
public class InputSystemController : MonoBehaviour
{
    [SerializeField] private GameObject _prefabDialog;

    private InputSystem_Actions _inputActions;
    private InputSystem_Actions.UIActions _uiAct;
    private InputSystem_Actions.PlayerActions _playerAct;

    private Player player;
    private RectTransform _canvas;
    private bool _canCancel = true;


    public void SetCanCancel()
    {
        _canCancel = true;
    }
    public void SetCannotCancel()
    {
        _canCancel = false;
    }


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
        //_playerAct.Attack.performed += context => AttackPerformed();
        _playerAct.Attack.canceled += context => AttackCanceled();
    }
    public void UnsubscribePlayer()
    {
        _playerAct.Interact.performed -= context => StartDialog();

        _playerAct.Move.started -= context => MoveStarted();
        _playerAct.Move.performed -= context => MovePerformed(context.ReadValue<Vector2>());
        _playerAct.Move.canceled -= context => MoveCanceled();

        _playerAct.Attack.started -= context => AttackStarted();
        //_playerAct.Attack.performed -= context => AttackPerformed();
        _playerAct.Attack.canceled -= context => AttackCanceled();
    }


    private void Awake()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        player = GetComponent<Player>();
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
    private async Task Delay()
    {
        await Task.Run(() => { while (!_canCancel) { }; return; });
    }


    private void StartDialog()
    {
        if (GameObject.FindGameObjectWithTag(_prefabDialog.tag)) return;
        GameObject obj = Instantiate(_prefabDialog, _canvas);
        Dialog txt = obj.GetComponentInChildren<Dialog>();
        txt.Path = "Dialog/greeting";
        txt.StartDialog("test");
    }

    private async void MoveStarted()
    {
        await Delay();
        //Debug.Log("MoveStart");
        player.SetBehaviorMoving();
    }
    private async void MovePerformed(Vector2 vector)
    {
        await Delay();
        //Debug.Log("MovePerf");
        if (player.IsMoving()) player.MoveVector = vector;
        else MoveCanceled();
    }
    private async void MoveCanceled()
    {
        await Delay();
        Debug.Log("MoveCancel");
        player.SetBehaviorByDefault();
    }
    private void AttackStarted()
    {
        Debug.Log("AttackStart");
        player.SetBehaviorAttacking();
        SetCannotCancel();
    }
    //private void AttackPerformed()
    //{
    //    //Debug.Log("AttackPerf");
    //}
    private async void AttackCanceled()
    {
        await Delay();
        Debug.Log("AttackCancel");
        if (player.IsIdle()) player.SetBehaviorIdle();
        else if (player.IsMoving()) player.SetBehaviorMoving();
        else player.SetBehaviorByDefault();
        SetCanCancel();
    }

    private void PlayPause()
    {
        ISimulationSystem.ReversePlayPause();
        player.UpdateAnimatorSpeed();
    }
    private void SpeedMore()
    {
        ISimulationSystem.SetSpeed(SimulationSpeedStates.Increase);
        player.UpdateAnimatorSpeed();
    }
    private void SpeedLess()
    {
        ISimulationSystem.SetSpeed(SimulationSpeedStates.Decrease);
        player.UpdateAnimatorSpeed();
    }
    private void Restart()
    {
        ISimulationSystem.SetState(SimulationStates.Stop);
    }
}