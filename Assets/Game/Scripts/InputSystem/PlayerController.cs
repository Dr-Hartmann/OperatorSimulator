using UnityEngine;
using Simulation;

namespace PlayerSpace
{
    public class PlayerController
    {
        #region ANIMATOR
        public void SetCanCancel()
        {
            _canCancel = true;
        }
        public void SetCannotCancel()
        {
            _canCancel = false;
        }
        #endregion

        #region CORE
        public void Init(Player player, float baseSpeed)
        {
            _player = GameObject.Instantiate(player, new Vector3(Random.value % 100f, Random.value % 100f), new(0, 0, 0, 0));
            _player.Init(baseSpeed);

            _inputActions = new();
            _uiAct = _inputActions.UI;
            _playerAct = _inputActions.Player;

            Enable();
        }
        private /*async*/ void ChangeBehavior()
        {
            //await Task.Run(() => { while (!_canCancel) { }; });

            if (_attack) _player.SetBehaviorAttacking();
            else if (_move) _player.SetBehaviorMoving();
            else _player.SetBehaviorIdle();
        }
        public void Enable()
        {
            SubscribeAll();
            _uiAct.Enable();
            _playerAct.Enable();
        }
        public void Disable()
        {
            UnsubscribeAll();
            _uiAct.Disable();
            _playerAct.Disable();
        }
        #endregion

        private void StartDialog()
        {

        }

        #region MOVE
        private void MoveStarted()
        {
            _move = true;
            ChangeBehavior();
        }
        private void MovePerformed(Vector2 vector)
        {
            _player.MoveVector = vector;
        }
        private void MoveCanceled()
        {
            _move = false;
            ChangeBehavior();
        }
        #endregion

        #region ATTACK
        private void AttackStarted()
        {
            _attack = true;
            ChangeBehavior();
        }
        private void AttackCanceled()
        {
            _attack = false;
            ChangeBehavior();
        }
        #endregion

        #region SIMULATION
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
        #endregion

        #region subscriptions
        private void SubscribeAll()
        {
            UnsubscribeAll();
            SubscribePlayer();
            SubscribeUI();
        }
        private void UnsubscribeAll()
        {
            UnsubscribePlayer();
            UnsubscribeUI();
        }
        private void SubscribeUI()
        {
            _uiAct.PlayPause.performed += context => PlayPause();
            _uiAct.SpeedMore.performed += context => SpeedMore();
            _uiAct.SpeedLess.performed += context => SpeedLess();
            _uiAct.Restart.performed += context => Restart();
        }
        private void UnsubscribeUI()
        {
            _uiAct.PlayPause.performed -= context => PlayPause();
            _uiAct.SpeedMore.performed -= context => SpeedMore();
            _uiAct.SpeedLess.performed -= context => SpeedLess();
            _uiAct.Restart.performed -= context => Restart();
        }
        private void SubscribePlayer()
        {
            //_playerAct.Interact.performed += context => StartDialog();

            _playerAct.Move.started += context => MoveStarted();
            _playerAct.Move.performed += context => MovePerformed(context.ReadValue<Vector2>());
            _playerAct.Move.canceled += context => MoveCanceled();

            _playerAct.Attack.started += context => AttackStarted();
            _playerAct.Attack.canceled += context => AttackCanceled();
        }
        private void UnsubscribePlayer()
        {
            //_playerAct.Interact.performed -= context => StartDialog();

            _playerAct.Move.started -= context => MoveStarted();
            _playerAct.Move.performed -= context => MovePerformed(context.ReadValue<Vector2>());
            _playerAct.Move.canceled -= context => MoveCanceled();

            _playerAct.Attack.started -= context => AttackStarted();
            _playerAct.Attack.canceled -= context => AttackCanceled();
        }
        #endregion

        #region variables
        private InputSystem_Actions _inputActions;
        private InputSystem_Actions.UIActions _uiAct;
        private InputSystem_Actions.PlayerActions _playerAct;
        private Player _player;
        private bool _canCancel = true;
        private bool _move = false;
        private bool _attack = false;
        #endregion
    }
}