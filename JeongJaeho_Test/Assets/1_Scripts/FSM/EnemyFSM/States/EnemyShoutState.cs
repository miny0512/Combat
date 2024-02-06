using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class EnemyShoutState : EnemyState<EnemyAIController>
{
    public EnemyShoutState(EnemyAIController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        owner.IsShoutFinished = false;
        owner.ExpectedVelocity = Vector3.zero;
        owner.Animator.Play(owner.ANIM_SHOUT);
        owner.EventHandler.OnShoutEndAction -= OnShoutEnd;
        owner.EventHandler.OnShoutEndAction += OnShoutEnd;
        owner.EventHandler.OnHitAction -= OnHitCheck;
        owner.EventHandler.OnHitAction += OnHitCheck;
        owner.StartCoroutine(CooldownTimer());
    }

    private void OnHitCheck()
    {
        if (owner.ShoutRangeCheck(out var hitObject))
        {
           var damageable = hitObject.GetComponent<ITakeDamageable>();
            DamageInfo dmgInfo = new DamageInfo(owner.gameObject, owner.ShoutDamage, (hitObject.transform.position - owner.ModelTransform.position).normalized * owner.ShoutKnockbackPower, Vector3.zero);
            damageable?.TakeDamage(dmgInfo);

            var dot = hitObject.gameObject.AddComponent<StunDebuff>();
            dot.Setup(owner.gameObject, hitObject.gameObject.GetComponent<Health>(), 0, 3f);
            dot.VFXSetup("Stun");
            dot.Play();
        }
        var vfxPos = owner.ShoutVfxPlayPosition.position + owner.ModelTransform.forward * 1f;
        var vfx = FxManager.Instance.GetVFX(owner.ShoutVfxName, vfxPos);
        vfx.transform.forward = owner.ModelTransform.forward;
    }

    public override void Exit()
    {
        owner.EventHandler.OnHitAction -= OnHitCheck;
        owner.IsShoutFinished = true;
    }

    private void OnShoutEnd()
    {
        owner.IsShoutFinished = true;
    }

    IEnumerator CooldownTimer()
    {
        float cooltime = owner.ShoutCoolTime;
        owner.ShoutCoolTimeElapsedTime = cooltime;
        while(owner.ShoutCoolTimeElapsedTime > 0)
        {
            owner.ShoutCoolTimeElapsedTime -= Time.deltaTime;
            yield return null;
        }
        owner.ShoutCoolTimeElapsedTime = 0f;
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
    }
}
