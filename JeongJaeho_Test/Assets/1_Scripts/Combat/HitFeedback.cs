using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeedback : MonoBehaviour
{
    [field: SerializeField] public string vfxName { get; private set; }
    [field: SerializeField] public bool IsInvincibility { get; private set; }
    [field: SerializeField] public float invincibilityTime { get; private set; }
    

    public bool Hit(Vector3 position)
    {
        if (IsInvincibility) return false;

        FxManager.Instance.GetVFX(vfxName, position);
        IsInvincibility = true;
        Invoke(nameof(InvincibilityTimeEnd), invincibilityTime);
        return true;
    }

    public void InvincibilityTimeEnd()
    {
        IsInvincibility = false;
    }
}
