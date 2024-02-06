using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class FxObjectPool
{
    private Transform _parentTransform;
    public ObjectPool<Effect> Pool { get; private set; }
    private readonly string DefaultPath = "VFX/";
    Effect prefab;

    public FxObjectPool(Transform parent, string path)
    {
        _parentTransform = parent;
        prefab = Resources.Load<Effect>(DefaultPath + path);
        Pool = new ObjectPool<Effect>(CreateFunc, ActionOnGet, ActionOnRelease);
    }

    public Effect CreateFunc()
    {
        var create = MonoBehaviour.Instantiate(prefab, _parentTransform);
        create.SetPool(Pool);
        create.Init();
        return create;
    }

    public void ActionOnGet(Effect effect)
    {
        effect.transform.parent = _parentTransform;
        effect.IsReleased = false;
        effect.gameObject.SetActive(true);
    }

    public void ActionOnRelease(Effect effect)
    {
        effect.gameObject.SetActive(false);
        effect.transform.parent = _parentTransform;
        effect.ResetTransform();
    }
}
