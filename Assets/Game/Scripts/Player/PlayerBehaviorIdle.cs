using UnityEngine;
public class PlayerBehaviorIdle : IPlayerBehavior
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
        Debug.Log("Enter IDLE");
    }

    public void Exit()
    {
        Debug.Log("Exit IDLE");
    }

    public void Update()
    {
        Debug.Log("Update IDLE");
    }
}
