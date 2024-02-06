using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMoveState : PlayerState<PlayerController>
{
    public PlayerMoveState(PlayerController pc) : base(pc) { }
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        float magnitude = owner.InputDirection.magnitude;
        float speed = Mathf.Lerp(owner.MovementStatus.Speed, owner.MovementStatus.Speed * owner.MovementStatus.DefaultRunMultiplier, magnitude / 1f);

        owner.ExpectedVelocity = owner.InputDirection * speed;
        owner.Animator.SetFloat(owner.ANIM_PARAMETER_FORWARDSPEED, magnitude);
    }
}
