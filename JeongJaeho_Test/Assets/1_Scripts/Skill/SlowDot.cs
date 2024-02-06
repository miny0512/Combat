using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SlowDot : Dot
{
    MovementStatus targetMovementStatus;
    protected override void EffectStart()
    {
        targetMovementStatus = target.GetComponent<MovementStatus>();
        if(targetMovementStatus != null )
        {
            targetMovementStatus.AddAdditiveSpeed(this, -2f);
        }
    }

    public override IEnumerator CoActionOverLifeTime()
    {
        for (int i = 0; i < (int)lifeTime; ++i)
        {
            if (target.IsDead) yield break;
            EffectTick();
            yield return new WaitForSeconds(1f);
        }
        EffectEnd();
        Destroy(this);
    }

    protected override void EffectEnd()
    {
        targetMovementStatus.RemoveAdditiveSpeed(this);
    }

    protected override void EffectTick()
    {
        DamageTarget();
    }
}
