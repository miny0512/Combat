using System;
using UnityEngine;

public abstract class PlayerState<OwnerType> : IState where OwnerType : MonoBehaviour
{
    public PlayerState(OwnerType owner)
    {
        this.owner = owner;
    }

    protected OwnerType owner;

    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedUpdate();
    public abstract void Update();
}
