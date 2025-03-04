using UnityEngine;
using System.Collections.Generic;
using System;
using Simulation;
using PlayerBehaviors;

namespace PlayerSpace
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 8f;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        public Vector2 MoveVector { get; set; }

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
        public void UpdateAnimatorSpeed()
        {
            float newSpeed = 1 * SimulationSystem.CurrentSpeed;
            if (newSpeed > 0) _animator.speed = newSpeed;
            else _animator.speed = -newSpeed;
        }

        public void EnterIdle()
        {
            _animator.SetBool(_isIdle, true);
            _animator.SetBool(_isMove, false);
        }
        public void UpdateIdle()
        {

        }
        public void ExitIdle()
        {

        }

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

        public void EnterMoving()
        {
            _animator.SetBool(_isIdle, false);
            _animator.SetBool(_isMove, true);
        }
        public void UpdateMoving()
        {
            _rigidbody.linearVelocity = MoveVector * _speed * SimulationSystem.CurrentSpeed;
            if (_rigidbody.linearVelocity.x > 0) _spriteRenderer.flipX = false;
            else _spriteRenderer.flipX = true;
        }
        public void ExitMoving()
        {
            _rigidbody.linearVelocity = new Vector2(0, 0);
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


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            InitBehaviors();
            UpdateAnimatorSpeed();
        }
        private void FixedUpdate()
        {
            if (_behaviorCurrent != null && SimulationSystem.IsPlayed)
            {
                _behaviorCurrent.Update();
            }
        }
        private void InitBehaviors()
        {
            _behaviorMap = new();

            _behaviorMap[typeof(PlayerBehaviorIdle)] = new PlayerBehaviorIdle(EnterIdle, ExitIdle, UpdateIdle);
            _behaviorMap[typeof(PlayerBehaviorMoving)] = new PlayerBehaviorMoving(EnterMoving, ExitMoving, UpdateMoving);
            _behaviorMap[typeof(PlayerBehaviorAttacking)] = new PlayerBehaviorAttacking(EnterAttacking, ExitAttacking, UpdateAttacking);

            SetBehaviorIdle();
        }
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
            var type = typeof(T);
            return _behaviorMap[type];
        }


        private PlayerBehavior _behaviorCurrent;
        private Dictionary<Type, PlayerBehavior> _behaviorMap;

        
        private const string IS_IDLE = "IsIdle";
        private const string IS_MOVE = "IsMove";
        private const string IS_ATTACK = "IsAttack";
        private int _isIdle = Animator.StringToHash(IS_IDLE);
        private int _isMove = Animator.StringToHash(IS_MOVE);
        private int _isAttack = Animator.StringToHash(IS_ATTACK);
    }
}