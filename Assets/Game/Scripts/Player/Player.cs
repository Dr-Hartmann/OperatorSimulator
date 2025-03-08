using UnityEngine;
using System.Collections.Generic;
using System;
using Simulation;
using PlayerBehaviors;

namespace PlayerSpace
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        /*[SerializeField] */private SpriteRenderer _spriteRenderer;
        /*[SerializeField] */private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;

        private const string IS_IDLE = "IsIdle";
        private const string IS_MOVE = "IsMove";
        private const string IS_ATTACK = "IsAttack";

        #region PUBLIC
        public void SetBehaviorIdle()
        {
            SetBehavior(GetBehavior<PlayerBehaviorIdle>());
        }
        public void SetBehaviorMoving()
        {
            SetBehavior(GetBehavior<PlayerBehaviorMoving>());
        }
        public void SetBehaviorAttacking()
        {
            SetBehavior(GetBehavior<PlayerBehaviorAttacking>());
        }
        public void SetBehaviorByDefault()
        {
            SetBehaviorIdle();
        }
        public void UpdateAnimatorSpeed(float multiplier = 1)
        {
            float newSpeed = SimulationSystem.CurrentSpeed * (SimulationSystem.IsPlayed ? 1f : 0f) * multiplier;
            _animator.speed = Mathf.Abs(newSpeed);
        }
        public bool IsIdle()
        {
            return _animator.GetBool(_isIdle);
        }
        public bool IsMoving()
        {
            return _animator.GetBool(_isMove);
        }
        public bool IsAttacking()
        {
            return _animator.GetBool(_isAttack);
        }
        #endregion

        #region CORE
        public void Init(float baseSpeed)
        {
            Speed = baseSpeed;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _behaviorMap = new();
            _behaviorMap[typeof(PlayerBehaviorIdle)] = new PlayerBehaviorIdle(EnterIdle, ExitIdle, UpdateIdle);
            _behaviorMap[typeof(PlayerBehaviorMoving)] = new PlayerBehaviorMoving(EnterMoving, ExitMoving, UpdateMoving);
            _behaviorMap[typeof(PlayerBehaviorAttacking)] = new PlayerBehaviorAttacking(EnterAttacking, ExitAttacking, UpdateAttacking);
            SetBehaviorByDefault();

            UpdateAnimatorSpeed();
        }
        private void FixedUpdate()
        {
            if (_behaviorCurrent != null && SimulationSystem.IsPlayed)
            {
                _behaviorCurrent.Update();
            }
        }
        #endregion

        #region IDLE
        public void EnterIdle()
        {
            _animator.SetBool(_isIdle, true);
        }
        public void UpdateIdle()
        {

        }
        public void ExitIdle()
        {
            _animator.SetBool(_isIdle, false);
        }
        #endregion

        #region ATTACK
        public void EnterAttacking()
        {
            _animator.SetBool(_isAttack, true);
        }
        public void UpdateAttacking()
        {

        }
        public void ExitAttacking()
        {
            _animator.SetBool(_isAttack, false);
        }
        #endregion

        #region MOVE
        public void EnterMoving()
        {
            _animator.SetBool(_isMove, true);
        }
        public void UpdateMoving()
        {
            _rigidbody.linearVelocity = MoveVector * Speed * SimulationSystem.CurrentSpeed;
            if (_rigidbody.linearVelocity.x > 0) _spriteRenderer.flipX = false;
            else _spriteRenderer.flipX = true;
        }
        public void ExitMoving()
        {
            _rigidbody.linearVelocity = new Vector2(0, 0);
            _animator.SetBool(_isMove, false);
        }
        #endregion

        #region BEHAVIOR
        private void SetBehavior(PlayerBehavior newBehavior)
        {
            if (_behaviorCurrent != null)
            {
                _behaviorCurrent.Exit();
            }
            _behaviorCurrent = newBehavior;
            _behaviorCurrent.Enter();
        }
        private PlayerBehavior GetBehavior<T>() where T : PlayerBehavior
        {
            return _behaviorMap[typeof(T)];
        }
        #endregion

        #region variables
        private PlayerBehavior _behaviorCurrent;
        private Dictionary<Type, PlayerBehavior> _behaviorMap;
        public Vector2 MoveVector { get; set; }
        private float Speed { get; set; }

        private int _isIdle = Animator.StringToHash(IS_IDLE);
        private int _isMove = Animator.StringToHash(IS_MOVE);
        private int _isAttack = Animator.StringToHash(IS_ATTACK);
        #endregion
    }
}