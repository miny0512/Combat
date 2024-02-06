using UnityEngine;

public class EnemyChaseState : EnemyState<EnemyAIController>
{
    public EnemyChaseState(EnemyAIController controller) : base(controller) { }

    public override void Enter()
    {

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
        var direction = (owner.DetectedPlayer.transform.position - owner.transform.position).normalized;
        var moveVec = direction * owner.MovementStatus.Speed;
        owner.Animator.SetFloat(owner.ANIM_PARAMETER_FORWARDSPEED, Mathf.Lerp(owner.Animator.GetFloat(owner.ANIM_PARAMETER_FORWARDSPEED), 1, Time.deltaTime * 10f));
        owner.ExpectedVelocity = direction.normalized * owner.MovementStatus.Speed;
        var forward = Vector3.SmoothDamp(owner.ModelTransform.forward, direction, ref curVel, 0.1f);
        owner.ModelTransform.forward = forward;
    }
}
