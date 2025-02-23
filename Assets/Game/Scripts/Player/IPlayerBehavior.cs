using UnityEngine;

public interface IPlayerBehavior
{
    void Attack();
    void Defense();

    void Enter();
    void Exit();
    void Update();
}