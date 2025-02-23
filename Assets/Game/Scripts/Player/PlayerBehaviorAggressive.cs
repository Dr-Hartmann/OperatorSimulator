using UnityEngine;

public class PlayerBehaviorAggressive : IPlayerBehavior
{
    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Defense()
    {
        throw new System.NotImplementedException();
    }

    public void Enter()
    {
        Debug.Log("Enter AGGRESSIVE");
    }

    public void Exit()
    {
        Debug.Log("Exit AGGRESSIVE");
    }

    public void Update()
    {
        Debug.Log("Update AGGRESSIVE");
    }
}

