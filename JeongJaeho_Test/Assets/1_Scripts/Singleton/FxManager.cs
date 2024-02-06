using System.Collections.Generic;
using UnityEngine;

public class FxManager : Singleton<FxManager> 
{
    private Dictionary<string, FxObjectPool> poolDict = new();
    private Transform poolParent;

    public Effect GetVFX(string name)
    {
        if(poolParent == null)
        {
            GameObject go = new GameObject("@PoolParent");
            go.transform.SetParent(this.transform);
            poolParent = go.transform;
        }

        if(poolDict.TryGetValue(name, out var output) == false)
        {
            output = new FxObjectPool(poolParent, name);
            poolDict.Add(name, output);
        }

        return output.Pool.Get();
    }

    public Effect GetVFX(string name, Vector3 position)
    {
        var vfx = GetVFX(name);
        vfx.transform.position = position;
        return vfx;
    }

    public Effect GetVFX(string name, Vector3 position, float duration)
    {
        var vfx = GetVFX(name, position);
        vfx.DelayRelease(duration);
        return vfx;
    }

    public void ReleaseVFX(Effect effect)
    {
        effect.Release();
    }
}
