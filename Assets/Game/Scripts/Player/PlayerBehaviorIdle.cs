using System;

namespace PlayerBehaviors
{
    public class PlayerBehaviorIdle : PlayerBehavior
    {
        public PlayerBehaviorIdle(Action enter, Action exit, Action update)
            : base(enter, exit, update) { }

    }
}