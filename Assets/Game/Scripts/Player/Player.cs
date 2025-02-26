using UnityEngine;
using System.Collections.Generic;
using System;
using SimulationCore;
using PlayerBehaviors;

namespace PlayerSpace
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 8f;

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
            float newSpeed = 1 * ISimulationSystem.CurrentSpeed;
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
            _rigidbody.linearVelocity = MoveVector * _speed * ISimulationSystem.CurrentSpeed;
            if (_rigidbody.linearVelocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;
            else GetComponent<SpriteRenderer>().flipX = true;
        }
        public void ExitMoving()
        {
            _rigidbody.linearVelocity = new Vector2(0, 0);
        }


        public bool IsIdle()
        {
            return _animator.GetBool(_isIdle) == true;
        }
        public bool IsMoving()
        {
            return _animator.GetBool(_isMove) == true;
        }
        public bool IsAttacking()
        {
            return _animator.GetBool(_isAttack) == true;
        }


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            InitBehaviors();
            UpdateAnimatorSpeed();
        }
        private void FixedUpdate()
        {
            if (_behaviorCurrent != null && ISimulationSystem.IsPlayed)
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

            SetBehaviorByDefault();
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

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private const string IS_IDLE = "IsIdle";
        private const string IS_MOVE = "IsMove";
        private const string IS_ATTACK = "IsAttack";
        private int _isIdle = Animator.StringToHash(IS_IDLE);
        private int _isMove = Animator.StringToHash(IS_MOVE);
        private int _isAttack = Animator.StringToHash(IS_ATTACK);
    }
}