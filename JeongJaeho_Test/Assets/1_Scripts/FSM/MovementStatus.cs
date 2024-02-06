using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementStatus : MonoBehaviour
{
    [SerializeField, Readonly] private float unclampedSpeed;
    public float DefaultSpeed;
    public float DefaultRunMultiplier;

    private void Awake()
    {
        unclampedSpeed = DefaultSpeed;
    }
    public float Speed { get
        {
            if(additiveSpeed.Count== 0) return DefaultSpeed;
            float sum = 0f;
            foreach (var val in additiveSpeed.Values) sum += val;
            return Mathf.Clamp(sum + DefaultSpeed, 0.2f, 10);
        } 
    }

    public void AddAdditiveSpeed(UnityEngine.Object sender, float speed)
    {
        additiveSpeed.Add(sender, speed);
        unclampedSpeed = unclampedSpeed +speed;
    }

    public void RemoveAdditiveSpeed(UnityEngine.Object sender)
    {
        unclampedSpeed = unclampedSpeed - additiveSpeed[sender];
        additiveSpeed.Remove(sender);
    }

    public Dictionary<UnityEngine.Object, float> additiveSpeed =new();
}
