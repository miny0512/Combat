using UnityEngine;

public class EnemyIdleState : EnemyState<EnemyAIController>
{
    public EnemyIdleState(EnemyAIController controller) : base(controller) { }

    public override void Enter()
    {
        owner.Animator.CrossFade(owner.ANIM_LOCOMOTION, 0.1f);
        owner.Animator.SetFloat(owner.ANIM_PARAMETER_FORWARDSPEED, 0f);
        owner.ExpectedVelocity = Vector3.zero;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    Vector3 curVel;
    public override void Update()
    {
        if(owner.DetectedPlayer != null)
        {
            var direction = (owner.DetectedPlayer.transform.position - owner.transform.position).normalized;
            var forward = Vector3.SmoothDamp(owner.ModelTransform.forward, direction, ref curVel, 0.1f);
            owner.ModelTransform.forward = forward;
        }
    }
}
