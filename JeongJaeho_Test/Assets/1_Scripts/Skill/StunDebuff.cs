using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StunDebuff : Dot
{
    IController controller;

    protected override void EffectStart()
    {
        controller = target.GetComponent<IController>();
        controller?.FreezeMovement(true);
    }

    protected override void EffectTick()
    {
        controller?.FreezeMovement(true);
    }

    public override IEnumerator CoActionOverLifeTime()
    {
        float elapsedTime = 0;
        while(elapsedTime < lifeTime)
        {
            elapsedTime += Time.deltaTime;
            if (target.IsDead) yield break;
            EffectTick();
            yield return null;
        }
        EffectEnd();
        Destroy(this);
    }

    protected override void EffectEnd()
    { 
        controller?.FreezeMovement(false);
    }
}
