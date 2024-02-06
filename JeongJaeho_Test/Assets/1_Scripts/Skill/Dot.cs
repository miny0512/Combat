using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public abstract class Dot : MonoBehaviour
{
    protected int tickDamage;    
    protected float lifeTime;
    protected Health target;

    protected GameObject sender;

    protected string vfxName;

    public void Setup(GameObject sender, Health target, int tickDamage, float lifeTime)
    {
        this.sender = sender;
        this.target = target;
        this.tickDamage = tickDamage;
        this.lifeTime = lifeTime;
    }

    public void VFXSetup(string name)
    {
        vfxName = name;
    }

    protected abstract void EffectStart();
    protected abstract void EffectTick();
    protected abstract void EffectEnd();

    public void Play()
    {
        EffectStart();
        StartCoroutine(CoActionOverLifeTime());
        if(vfxName != null)
        {
            var vfx = FxManager.Instance.GetVFX(vfxName, target.transform.position, lifeTime);
            vfx.transform.SetParent(target.transform);
            vfx.transform.localScale = target.transform.localScale;
        }
    }

    public abstract IEnumerator CoActionOverLifeTime();

    protected void DamageTarget()
    {
        target.TakeDamage(new DamageInfo(sender, tickDamage, Vector3.zero));
    }
}
