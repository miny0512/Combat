
using UnityEngine;

public struct DamageInfo
{
    public DamageInfo(GameObject sender, int dmg, Vector3 hitPosition)
    {
        Sender = sender;
        Damage = dmg;
        IsKnockbackAttack = false;
        KnockbackForce = Vector3.zero;
        HitPosition = hitPosition;
    }

    public DamageInfo(GameObject sender, int dmg,Vector3 knockbackForce, Vector3 hitPosition)
    {
        Sender = sender;
        Damage = dmg;
        KnockbackForce = knockbackForce;
        IsKnockbackAttack = true;
        HitPosition = hitPosition;
    }

    public GameObject Sender;
    public Vector3 HitPosition;
    public int Damage;
    public bool IsKnockbackAttack;
    public Vector3 KnockbackForce;
}
