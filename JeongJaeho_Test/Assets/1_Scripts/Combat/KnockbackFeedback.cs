using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HitFeedback))]
public class KnockbackFeedback : MonoBehaviour
{
    public bool Knockbackable = true;
    [SerializeField, Readonly] private float _stunTime;
    private IController _controller;
    private WaitForSeconds _wfs;
    private Rigidbody _rigidbody;
    public bool IsKnockback { get;private set; }

    private void Awake()
    {
        _stunTime = GetComponent<HitFeedback>().invincibilityTime;
        _wfs = new WaitForSeconds(_stunTime);
        _controller = GetComponent<IController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Knockback(GameObject sender, Vector3 knockbackForce)
    {
        if (Knockbackable == false) return;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(knockbackForce,ForceMode.Impulse);
        IsKnockback = true;
        StartCoroutine(KnockbackEnd(sender));
    }

    public IEnumerator KnockbackEnd(GameObject sender)
    {
        yield return _wfs;
        _rigidbody.velocity = Vector3.zero;
        IsKnockback = false;
    }
}
