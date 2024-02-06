
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttackState : PlayerState<PlayerController>
{
    public PlayerAttackState(PlayerController pc) : base(pc) { }

    bool isCombo = false;
    private int currentCombo = 0;
    public override void Enter()
    {
        isCombo = false;
        currentCombo = 0;
        owner.ExpectedVelocity = Vector3.zero;
        owner.IsAttackEnd = false;
        owner.Animator.Play(owner.ANIM_ATTACK[currentCombo]);
        owner.EventHandler.OnAttackEndAction -= OnFinishedAttack;
        owner.EventHandler.OnAttackEndAction += OnFinishedAttack;
        owner.EventHandler.OnEffectPlayAction -= OnEffectPlay;
        owner.EventHandler.OnEffectPlayAction += OnEffectPlay;
        DamageEnemy();
        owner.StartCoroutine(CameraEffector.Instance.ShakeCamera(0.1f, 2f));
    }

    private void DamageEnemy()
    {
        if (owner.HitCheck(out var hit) == true)
        {
            var damageable = hit.GetComponent<ITakeDamageable>();
            var hitDir = (hit.transform.position - owner.transform.position).normalized;
            damageable?.TakeDamage(new DamageInfo(owner.gameObject, owner.AttackPower, 5 * hitDir, Vector3.zero));
        }
        AttackDash();
    }

    private void AttackDash()
    {
        owner.Rigidbody.AddForce(owner.ModelTransform.forward * 20, ForceMode.Impulse);
    }
    private void OnEffectPlay()
    {
        var vfx = FxManager.Instance.GetVFX(owner.SlashParticleName[currentCombo]);
        vfx.transform.position = owner.SlashParticleTransform[currentCombo].position;
        vfx.transform.rotation = owner.SlashParticleTransform[currentCombo].rotation;
        // owner.SlashParticle[currentCombo].Play();
    }

    private void OnFinishedAttack()
    {
        if (isCombo == false) owner.IsAttackEnd = true;
        else
        {
            isCombo = false;
            currentCombo++;
            currentCombo %= 3;
            owner.Animator.Play(owner.ANIM_ATTACK[currentCombo]);
            DamageEnemy();
            owner.StartCoroutine(CameraEffector.Instance.ShakeCamera(0.1f, 1f));
        }
    }

    public override void Exit()
    {
        owner.EventHandler.OnAttackEndAction -= OnFinishedAttack;
        owner.EventHandler.OnEffectPlayAction -= OnEffectPlay;
    }

    public override void FixedUpdate()
    {
    }
    
    public override void Update()
    {
        float normalizedTime = owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if(Input.GetMouseButtonDown(0) && 0.3f < normalizedTime && normalizedTime < 0.7f) 
        {
            isCombo = true;
        }
    }
}
