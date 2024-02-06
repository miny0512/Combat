using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Shout,
    Return
}

public class EnemyStateMachine : StateMachine<EnemyState, EnemyAIController>
{
    public EnemyStateMachine(EnemyAIController controller) : base(controller) { }

    protected override void Init(EnemyAIController owner)
    {
        this.owner = owner;
        factory = new EnemyStateFactory(owner);
        InitTransitions();
        SetDefaultState();
    }

    protected override void InitTransitions()
    {
        // Idle -> ~
        AddTransition(factory[EnemyState.Idle], factory[EnemyState.Chase], () => owner.RiskGuage == 1f);
        AddTransition(factory[EnemyState.Idle], factory[EnemyState.Return], () => owner.RiskGuage != 1f && owner.IsReturned==false);
        // Chase -> ~
        AddTransition(factory[EnemyState.Chase], factory[EnemyState.Attack], () => owner.IsAttackEnd == true && owner.IsEnemyInAttackRange() == true);
        AddTransition(factory[EnemyState.Chase], factory[EnemyState.Shout], () => owner.IsShoutable == true);
        AddTransition(factory[EnemyState.Chase], factory[EnemyState.Return], () => owner.RiskGuage != 1f);

        AddTransition(factory[EnemyState.Return], factory[EnemyState.Idle], () => owner.IsReturned);

        AddTransition(factory[EnemyState.Attack], factory[EnemyState.Idle], () => owner.IsAttackEnd);

        AddTransition(factory[EnemyState.Shout], factory[EnemyState.Idle], () => owner.IsShoutFinished);
    }

    protected override void SetDefaultState()
    {
        ChangeState(factory[EnemyState.Idle]);
    }

}
