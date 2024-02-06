using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState, OwnerType> where EState : Enum where OwnerType : MonoBehaviour
{
    public StateMachine(OwnerType owner) { Init(owner); }

    protected OwnerType owner;
    protected StateFactory<EState> factory;
    protected Dictionary<EState, Transition> states;
    protected List<Transition> transitions;

    public bool IsStopped { get; set; } = false;

    private IState _currentState;

    public void SetState(EState state)
    {
        ChangeState(factory[state]);
    }

    public void ChangeState(IState newState)
    {
        if(_currentState == newState) return;
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update()
    {
        if (IsStopped == true) return;

        ConditionCheck();
        _currentState?.Update();
    }

    public void FixedUpdate()
    {
        if(IsStopped == true) return;

        _currentState?.FixedUpdate();
    }

    private void ConditionCheck()
    {
        foreach(var i in transitions) 
        {
            if (i.From != _currentState) continue;
            if (i.CheckTransitionCondition())
            {
                ChangeState(i.To);
                return;
            }
        }
    }

    protected void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (transitions == null) transitions = new();
        transitions.Add(new Transition(from, to, condition));
    }
    protected abstract void Init(OwnerType owner);
    protected abstract void InitTransitions();
    protected abstract void SetDefaultState();
}
