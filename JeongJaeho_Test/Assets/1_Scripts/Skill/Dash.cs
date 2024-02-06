using System.Collections;
using UnityEngine;

public class Dash : SkillBase
{
    [field: Space(10), HeaderColor("Dash", ColorType.WHITE)]
    [field: SerializeField] public float DashSlashRange { get; private set; }
    [field: SerializeField] public int DashSlashDamage { get; private set; }
    [field: SerializeField] public float DashPower { get; private set; }
    [field: SerializeField, Readonly] public Vector3 CurrentDirection { get; private set; }

    public override void SkillEnter()
    {
        base.SkillEnter();
        CurrentDirection = Player.ModelTransform.forward;
        Player.ExpectedVelocity = CurrentDirection * DashPower;
        CurrentDuration = 0f;
        StartCoroutine(CoActivate());
        Player.Animator.Play(Player.ANIM_DASHATTACK);
    }

    public override void SkillExit()
    {
        Player.ExpectedVelocity = Vector3.zero;
        
    }

    public override IEnumerator CoActivate()
    {
        var lens = CameraEffector.Instance.LensDistortion;
        var chro = CameraEffector.Instance.ChromaticAberration;
        lens.active = true;
        chro.active = true;

        while (CurrentDuration < Duration)
        {
            CurrentDuration += Time.deltaTime;
            float ratio = CurrentDuration / Duration;
            lens.intensity.value = Mathf.Lerp(-60, 0, ratio);
            chro.intensity.value = Mathf.Lerp(0.7f, 0, ratio);
            yield return null;
        }

        lens.active = false;
        chro.active = false;

        var height = Player.GetComponent<CapsuleCollider>().height;
        var spawnPos = Player.transform.position + Vector3.up * height / 2f;
        var vfx = FxManager.Instance.GetVFX(base.VfxPrefabName, spawnPos);
        vfx.transform.forward = Player.ModelTransform.forward;
        var direction = vfx.transform.forward * 10f;
        vfx.MoveOverLifeTime(vfx.transform.position + direction);

        var hit = Physics.OverlapSphere(transform.position, DashSlashRange, Player.WhatIsEnemy);
        if (hit.Length != 0)
        {
            var damageable = hit[0].GetComponent<ITakeDamageable>();
            damageable?.TakeDamage(new DamageInfo(Player.gameObject, DashSlashDamage, Player.ModelTransform.forward * 10f));
        }
        FinishSkill();
    }
}
