using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class ReadonlyAttribute : PropertyAttribute
{
    public ReadonlyAttribute()
    {
    }
}
