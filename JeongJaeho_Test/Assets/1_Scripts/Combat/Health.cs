using System;
using UnityEngine;

public class Health : MonoBehaviour, ITakeDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    public event Action<GameObject> OnDie;
    public event Action<float> OnHealthChanged;

    public int MaxHealth => _maxHealth;
    public bool IsDead { get; private set; }
    private float _hpRatio => _currentHealth / (float)_maxHealth;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // 체력 초기화
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_hpRatio);
    }

    public void Heal(int amount)
    {
        if(IsDead) return;
        _currentHealth = Mathf.Clamp(amount + _currentHealth, 0, _maxHealth);
        OnHealthChanged?.Invoke(_hpRatio);
    }

    public void TakeDamage(DamageInfo dmgInfo)
    {
        if (IsDead) return;

        int damagedHp = _currentHealth - dmgInfo.Damage;
        var hitFeedback = GetComponent<HitFeedback>();

        if(hitFeedback != null)
        {
            // 무적이면 false
            if (hitFeedback.Hit(transform.position) == false) return;
        }

        _currentHealth = Math.Clamp(damagedHp, 0, _maxHealth);

        OnHealthChanged?.Invoke(_hpRatio);

        // Die
        if (_currentHealth == 0)
        {
            OnDie?.Invoke(dmgInfo.Sender);
            IsDead = true;
            return;
        }

        // 넉백 공격
        if (dmgInfo.IsKnockbackAttack)
        {
            var knockback = GetComponent<KnockbackFeedback>();
            if(knockback != null)
            {
                knockback.Knockback(dmgInfo.Sender, dmgInfo.KnockbackForce);
            }
        }

        _currentHealth = damagedHp;
    }
}
