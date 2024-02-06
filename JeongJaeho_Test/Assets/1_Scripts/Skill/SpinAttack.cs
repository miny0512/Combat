using System;
using System.Collections;
using UnityEngine;

public class SpinAttack : SkillBase
{
    [field: Space(10), HeaderColor("Spin", ColorType.WHITE)]
    [field: SerializeField] public float SpinRange { get; private set; }
    [field: SerializeField] public int SpinDamage { get; private set; }
    [field: SerializeField] public float SpinKnockbackPower { get; private set; }
    [field: SerializeField, Tooltip("초당 몇 번 공격하는지")] public int DamgePerSecond { get; private set; }
    [field: SerializeField] public float SpinMoveSpeed { get; private set; }
    [field: SerializeField, Readonly] public Vector3 CurrentDirection { get; private set; }
    [field: SerializeField, Readonly] public Effect vfx { get; private set; }
    [field: SerializeField, Readonly] public float PlayerMiddle { get; private set; }
    private Coroutine currentCoroutine;
    public override void SkillEnter()
    {
        base.SkillEnter();

        DamgePerSecond = Math.Clamp(DamgePerSecond, 1, 20);
        CurrentDuration = 0f;
        PlayerMiddle = Player.GetComponent<CapsuleCollider>().height * 0.5f;
        vfx = FxManager.Instance.GetVFX(VfxPrefabName, Player.transform.position, Duration);
        currentCoroutine = StartCoroutine(CoActivate());
        Player.Animator.Play(Player.ANIM_SPINATTACK);
    }

    protected override void OnCancledSkill()
    {
        base.OnCancledSkill();
        StopCoroutine(currentCoroutine);
        FxManager.Instance.ReleaseVFX(vfx);
    }

    public override IEnumerator CoActivate()
    {
        float checkTime = 1 / (float)DamgePerSecond;
        float checkElapsedTime = 0f;
        while (CurrentDuration < Duration)
        {
            if (checkElapsedTime == 0)
            {
                if(Hit(out var hit) == true)
                {
                    var damageable = hit.GetComponent<ITakeDamageable>();
                    var knockbackDir = (hit.transform.position - Player.transform.position).normalized;
                    DamageInfo dmgInfo = new DamageInfo(Player.gameObject, SpinDamage, knockbackDir * SpinKnockbackPower, hit.transform.position);
                    damageable?.TakeDamage(dmgInfo);
                }
            }
            checkElapsedTime += Time.deltaTime;
            if (checkElapsedTime > checkTime) checkElapsedTime = 0f;
            CurrentDuration += Time.deltaTime;
            var moveVec = Player.InputDirection * SpinMoveSpeed;
            Player.ExpectedVelocity = moveVec;

            if (vfx.IsReleased == false)
            {
                vfx.transform.position = Player.Rigidbody.position + Vector3.up * PlayerMiddle;
            }

            yield return null;
        }
        FinishSkill();
    }

    private bool Hit(out GameObject hitObject)
    {
        var hit = Physics.OverlapSphere(Player.transform.position, SpinRange, Player.WhatIsEnemy);
        if (hit.Length != 0)
        {
            hitObject = hit[0].gameObject;
           return true;
        }
        hitObject = null;
        return false;
    }
}
