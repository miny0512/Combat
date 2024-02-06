using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public event Action OnHitAction;
    public event Action OnAttackEndAction;
    public event Action OnShoutEndAction;

    private void OnAttackEnd()
    {
        OnAttackEndAction?.Invoke();
    }

    private void OnHitCheck()
    {
        OnHitAction?.Invoke();
    }

    private void OnShoutEnd()
    {
        OnShoutEndAction?.Invoke();
    }
}
