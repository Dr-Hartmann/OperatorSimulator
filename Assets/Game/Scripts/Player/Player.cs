using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Dictionary<Type, IPlayerBehavior> behaviorMap;
    private IPlayerBehavior behaviorCurrent;

    private void Start()
    {
        InitBehaviors();
        SetBehaviorByDefault();
    }

    private void Update()
    {
        if (behaviorCurrent != null)
        {
            behaviorCurrent.Update();
        }
    }

    private void InitBehaviors()
    {
        behaviorMap = new();

        behaviorMap[typeof(PlayerBehaviorIdle)] = new PlayerBehaviorIdle();
        behaviorMap[typeof(PlayerBehaviorActive)] = new PlayerBehaviorActive();
        behaviorMap[typeof(PlayerBehaviorAggressive)] = new PlayerBehaviorAggressive();
    }

    private void SetBehavior(IPlayerBehavior newBehavior)
    {
        if(behaviorCurrent != null)
        {
            behaviorCurrent.Exit();
        }

        behaviorCurrent = newBehavior;
        behaviorCurrent.Enter();
    }

    private void SetBehaviorByDefault()
    {
        SetBehaviorIdle();
    }

    private IPlayerBehavior GetBehavior<T>() where T : IPlayerBehavior
    {
        var type = typeof(T);
        return behaviorMap[type];
    }

    public void SetBehaviorIdle()
    {
        var behavoir = GetBehavior<PlayerBehaviorIdle>();
        SetBehavior(behavoir);
    }

    public void SetBehaviorActive()
    {
        var behavoir = GetBehavior<PlayerBehaviorActive>();
        SetBehavior(behavoir);
    }

    public void SetBehaviorAggressive()
    {
        var behavoir = GetBehavior<PlayerBehaviorAggressive>();
        SetBehavior(behavoir);
    }
}
