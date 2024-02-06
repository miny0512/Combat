using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DotDebuff : SkillBase
{
    public override void SkillEnter()
    {
        base.SkillEnter();

        var test = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Enemy"));
        if (test.Length == 0)
        {
            FinishSkill();
            return;
        }
        var dot = test[0].AddComponent<SlowDot>();
        dot.Setup(Player.gameObject, test[0].GetComponent<Health>(), MaxDamage, Duration);
        dot.VFXSetup(VfxPrefabName);
        dot.Play();

        FinishSkill();
    }

    public override void SkillExit()
    {
    }
}
