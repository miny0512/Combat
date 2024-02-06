using UnityEngine;

public class PlayerIdleState : PlayerState<PlayerController>
{
    public PlayerIdleState(PlayerController pc) : base(pc) { }

    public override void Enter()
    {
        owner.ExpectedVelocity = Vector3.zero;
        owner.Animator.SetFloat(owner.ANIM_PARAMETER_FORWARDSPEED, 0);
        owner.Animator.CrossFade(owner.ANIM_LOCOMOTION, 0.1f);
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }


    public override void Update()
    {
    }
}
