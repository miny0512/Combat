using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [field: HeaderColor("SkillBase", ColorType.WHITE)]
    [field: SerializeField] public PlayerController Player { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string VfxPrefabName { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField, Readonly] public float CurrentDuration { get; protected set; }
    [field: SerializeField] public int MinDamage { get; private set; }
    [field: SerializeField] public int MaxDamage { get; private set; }
    [field: SerializeField] public float CoolDownTime { get; private set; }
    [field: SerializeField] public float CurrentCoolTime { get; private set; }
    [field: SerializeField] public bool IsSkillFinished { get; protected set; }

    public bool IsReady => CurrentCoolTime == 0;

    public event Action<float> OnSkillCooltimeChanged;
    public virtual void SkillEnter()
    {
        StartCoolTime();
        IsSkillFinished = false;
    }
    public virtual void SkillExit()
    {
        if(IsSkillFinished == false)
        {
            OnCancledSkill();
        }
    }


    // 스킬 종요할 때  호출
    protected void FinishSkill()
    {
        Player.IsActiveSkill = false;
        Player.CurrentSkill = null;
        IsSkillFinished = true;
    }

    protected virtual void OnCancledSkill()
    {
        FinishSkill();
    }

    public void UseSkill()
    {
        if (IsReady == false) return;

        Player.IsActiveSkill = true;
        Player.CurrentSkill = this;
    }

    public virtual void Activate() { }
    public virtual IEnumerator CoActivate() { yield break; }

    public void StartCoolTime()
    {
        StartCoroutine(CooldownTimer());
    }

    private IEnumerator CooldownTimer()
    {
        CurrentCoolTime = CoolDownTime;
        while (CurrentCoolTime > 0)
        {
            CurrentCoolTime -= Time.deltaTime;
            OnSkillCooltimeChanged?.Invoke(CurrentCoolTime / CoolDownTime);
            yield return null;
        }
        CurrentCoolTime = 0f;
    }

}
