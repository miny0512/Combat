using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyReturnState : EnemyState<EnemyAIController>
{
    public EnemyReturnState(EnemyAIController controller) : base(controller)
    {
    }

    private Vector3 returnDirection;
    public override void Enter()
    {
        owner.ExpectedVelocity = Vector3.zero;
        owner.Animator.SetFloat(owner.ANIM_PARAMETER_FORWARDSPEED, 1f);
        returnDirection = (owner.ReturnPosition - owner.Rigidbody.position).normalized;
        owner.ModelTransform.forward = returnDirection;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        owner.Rigidbody.position = Vector3.MoveTowards(owner.Rigidbody.position, owner.ReturnPosition, Time.deltaTime * owner.MovementStatus.DefaultSpeed * 2f);
    }

    Vector3 curVel;
    public override void Update()
    {
        owner.Health.Heal((int)(owner.Health.MaxHealth * 0.2f));

        var direction = (owner.ReturnPosition - owner.transform.position).normalized;
        var forward = Vector3.SmoothDamp(owner.ModelTransform.forward, direction, ref curVel, 0.1f);
        owner.ModelTransform.forward = forward;
    }
}
