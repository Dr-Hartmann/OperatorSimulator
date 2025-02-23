using UnityEngine;

public class PlayerBehaviorActive : IPlayerBehavior
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
        Debug.Log("Enter ACTIVE");
    }

    public void Exit()
    {
        Debug.Log("Exit ACTIVE");
    }

    public void Update()
    {
        Debug.Log("Update ACTIVE");
    }
}