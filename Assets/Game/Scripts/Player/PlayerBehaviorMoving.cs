using System;

namespace PlayerBehaviors
{
    public class PlayerBehaviorMoving : PlayerBehavior
    {
        public PlayerBehaviorMoving(Action enter, Action exit, Action update)
            : base(enter, exit, update) { }
    }
}