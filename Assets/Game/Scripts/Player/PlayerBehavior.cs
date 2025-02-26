using System;

namespace PlayerBehaviors
{
    public abstract class PlayerBehavior
    {
        private Action _enter;
        private Action _exit;
        private Action _update;

        public PlayerBehavior(Action enter, Action exit, Action update)
        {
            _enter = enter;
            _exit = exit;
            _update = update;
        }
        public virtual void Enter()
        {
            _enter();
        }
        public virtual void Exit()
        {
            _exit();
        }
        public virtual void Update()
        {
            _update();
        }
    }
}