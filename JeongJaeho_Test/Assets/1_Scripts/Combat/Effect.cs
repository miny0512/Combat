using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Effect : MonoBehaviour
{
    ObjectPool<Effect> pool;
    public float Duration { get; private set; }
    public bool IsPerformed { get; private set; }
    public bool IsReleased { get; set; }
    public ParticleSystem[] Particles { get; private set; }

    private Vector3 originPos;
    private Vector3 originScale;
    private Quaternion originRot;
    public void Init()
    {
        float max = float.MinValue;
        Particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var i in Particles)
        {
            max = Mathf.Max(max, i.main.duration);
        }
        Duration = max;

        originPos = transform.position;
        originScale = transform.localScale;
        originRot = transform.rotation;
    }

    public void ResetTransform()
    {
        transform.position= originPos;
        transform.rotation= originRot;
        transform.localScale= originScale;
    }

    public void SetPool(ObjectPool<Effect> pool)
    {
        this.pool = pool;
    }

    private void Update()
    {
        if (IsReleased == true) return;

        IsPerformed = true;

        foreach(var i in Particles)
        {
            if (i.isPlaying)
            {
                IsPerformed = false;
                break;
            }
        }

        if(IsPerformed == true)
        {
            Release();
        }
    }

    public void MoveOverLifeTime(Vector3 endPos)
    {
        StartCoroutine(CoMoveOverLifeTime(transform.position, endPos));
    }

    private IEnumerator CoMoveOverLifeTime(Vector3 currentPos, Vector3 endPos)
    {
        float elapsedTime = 0;
        while (elapsedTime < Duration)
        {
            if (IsReleased) yield break;

            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, endPos, elapsedTime / Duration);
            yield return null;
        }
    }
    public void DelayRelease(float delayTime)
    {
        Invoke("Release", delayTime);
    }

    public void Release()
    {
        if(IsReleased == true) return;

        pool.Release(this);
        IsReleased = true;
    }
}
