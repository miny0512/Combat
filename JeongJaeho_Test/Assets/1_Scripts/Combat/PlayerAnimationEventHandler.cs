using System;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public event Action OnAttackEndAction;
    public event Action OnEffectPlayAction;

    public void OnAttackEnd()
    {
        OnAttackEndAction?.Invoke();
    }

    public void OnEffectPlay()
    {
        OnEffectPlayAction?.Invoke();
    }
}
