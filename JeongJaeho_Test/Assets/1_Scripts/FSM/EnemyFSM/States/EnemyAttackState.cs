using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : EnemyState<EnemyAIController>
{
    public EnemyAttackState(EnemyAIController controller) : base(controller) 
    {
        _wfs = new WaitForSeconds(controller.AttackDelay);
    }

    private WaitForSeconds _wfs;
    private GameObject hitObject;

    public override void Enter()
    {
        owner.IsAttackEnd = false;
        owner.Animator.Play(owner.ANIM_ATTACK);
        owner.ExpectedVelocity = Vector3.zero;
        var forward = (owner.DetectedPlayer.transform.position - owner.transform.position).normalized;
        forward.y = 0f;
        owner.ModelTransform.forward = forward;
        owner.GetComponent<KnockbackFeedback>().Knockbackable = false;
        owner.EventHandler.OnAttackEndAction -= OnAttackEnd;
        owner.EventHandler.OnAttackEndAction += OnAttackEnd;
        owner.EventHandler.OnHitAction -= HitCheck;
        owner.EventHandler.OnHitAction += HitCheck;
    }

    public override void Exit()
    {
        owner.GetComponent<KnockbackFeedback>().Knockbackable = true;
        owner.EventHandler.OnAttackEndAction -= OnAttackEnd;
        owner.EventHandler.OnHitAction -= HitCheck;
        owner.IsAttackEnd = true;
    }

    public void OnAttackEnd()
    {
        owner.GetComponent<KnockbackFeedback>().Knockbackable = true;
        owner.StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return _wfs;
        owner.IsAttackEnd = true;
    }

    public void HitCheck()
    {
        float radius = owner.Collider.radius * 2f;
        var forward = owner.ModelTransform.forward;
        Vector3 forwardPosition = owner.transform.position + forward * radius;

        var result = Physics.OverlapBox(forwardPosition, Vector3.one, owner.ModelTransform.rotation, owner.WhatIsPlayer);
        if (result.Length == 0)
        {
            return;
        }

        var damageable = result[0].GetComponent<ITakeDamageable>();

        var knockbackForce = (result[0].transform.position - owner.transform.position).normalized;
        knockbackForce *= 20f;
        DamageInfo dmgInfo = new DamageInfo(owner.gameObject, 
            owner.AttackPower,
            knockbackForce,
            Vector3.zero
            );
        damageable.TakeDamage(dmgInfo);
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
    }
}
