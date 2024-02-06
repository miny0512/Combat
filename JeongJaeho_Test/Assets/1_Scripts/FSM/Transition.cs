using System;
using Unity.VisualScripting;

public class Transition
{
    public Transition(IState from, IState to, Func<bool> condition)
    {
        this.condition = condition;
        From = from;
        To = to;
    }

    public IState From{get; private set;}
    public IState To { get; private set; }
    private Func<bool> condition;
    public bool CheckTransitionCondition() => condition.Invoke();
}
