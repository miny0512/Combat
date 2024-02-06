using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private EnemyStateMachine _stateMachine;
    [field: SerializeField] public CinemachineVirtualCamera FrontViewCamera { get; private set; }
    [field: SerializeField] public Material Material { get; private set; }
    [field: SerializeField] public Transform ModelTransform { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public CapsuleCollider Collider { get; private set; }
    [field: SerializeField] public KnockbackFeedback KnockbackFeedback{ get; private set; }
    [field: SerializeField] public EnemyAnimationEventHandler EventHandler { get; private set; }

    [field: SerializeField] public Vector3 ReturnPosition { get; private set; }
    [field: SerializeField] public float RiskGuage { get; private set; }
    [field: SerializeField] public float SearchRange { get; private set; }
    public bool IsReturned => transform.position == ReturnPosition;

    [field: SerializeField] public LayerMask WhatIsPlayer { get; private set; }
    [field: SerializeField] public GameObject DetectedPlayer { get; private set; }
    [field: SerializeField] public int AttackPower { get; private set; }
    [field: SerializeField] public float StunTime { get; private set; }
    [field: SerializeField] public float AttackDelay { get; private set; }
    [field: SerializeField] public MovementStatus MovementStatus { get; private set; }
    [field: SerializeField] public float ChaseRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public bool RangeDebugging { get; private set; }
    [field: SerializeField] public bool IsStopped { get; private set; }
    [field: SerializeField] public bool IsAttackEnd { get; set; } = true;
    [field: SerializeField] public bool FreezePosition { get; set; }
    [field: Space(20)]

    [field:HeaderColor("Skill", ColorType.WHITE)]
    [field: SerializeField] public Transform ShoutVfxPlayPosition { get; private set; }
    [field: SerializeField] public string ShoutVfxName { get; private set; }
    [field: SerializeField] public int ShoutDamage { get; private set; }
    [field: SerializeField] public float ShoutKnockbackPower{ get; private set; }
    [field: SerializeField] public float ShoutCoolTime { get; private set; }
    [field: SerializeField, Readonly] public float ShoutCoolTimeElapsedTime { get; set; }
    [field: SerializeField, Readonly] public bool IsShoutFinished { get; set; } = false;
    public bool IsShoutable { get { return ShoutCoolTimeElapsedTime == 0 && ShoutRangeCheck(out var col); } }

    [SerializeField] private Vector3 _expectedVelocity;
    public Vector3 ExpectedVelocity { get { return _expectedVelocity; } set { _expectedVelocity = value; } }
    public float ExpectedVelocityX { get { return _expectedVelocity.x; } set { _expectedVelocity.x = value; } }
    public float ExpectedVelocityY { get { return _expectedVelocity.y; } set { _expectedVelocity.y = value; } }
    public float ExpectedVelocityZ { get { return _expectedVelocity.z; } set { _expectedVelocity.z = value; } }


    // Anim Hashing
    // Param
    public readonly int ANIM_PARAMETER_FORWARDSPEED = Animator.StringToHash("ForwardSpeed");
    // Anim clip
    public readonly int ANIM_LOCOMOTION = Animator.StringToHash("Locomotion");
    public readonly int ANIM_VICTORY = Animator.StringToHash("Shout");
    public readonly int ANIM_ATTACK = Animator.StringToHash("Attack");
    public readonly int ANIM_SHOUT = Animator.StringToHash("Shout");
    public readonly int ANIM_HIT = Animator.StringToHash("Hit");
    public readonly int ANIM_DEAD = Animator.StringToHash("Dead");

    public event Action<float> OnRiskGuageChanged;

    void Awake()
    {
        ReturnPosition = transform.position;

        KnockbackFeedback = GetComponent<KnockbackFeedback>();
        MovementStatus = GetComponent<MovementStatus>();
        EventHandler = GetComponent<EnemyAnimationEventHandler>();
        Health = GetComponent<Health>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();

        Material.SetFloat("_SplitValue", 4f);
        _stateMachine = new EnemyStateMachine(this);
        IsAttackEnd = true;
    }

    private void OnEnable()
    {
        Health.OnDie -= OnDie;
        Health.OnDie += OnDie;

        Health.OnDie -= GameManager.Instance.OnGameSet;
        Health.OnDie += GameManager.Instance.OnGameSet;
    }
    private void OnDisable()
    {
        Material.SetFloat("_SplitValue", 4f);   
        Health.OnDie -= OnDie;
        Health.OnDie -= GameManager.Instance.OnGameSet;
    }

    private void OnDestroy()
    {
        Material.SetFloat("_SplitValue", 4f);
    }

    void Update()
    {
        if (IsStopped) return;
        RiskGuage = Mathf.Clamp01(GetRiskGuage());
        OnRiskGuageChanged?.Invoke(RiskGuage);
        _stateMachine.Update();
        HandleMove();
    }

    private float GetRiskGuage()
    {
        if(IsEnemyInRange(SearchRange, out var enemy))
        {
            DetectedPlayer = enemy;
            var chaseRangePos = transform.position + (DetectedPlayer.transform.position - transform.position).normalized * ChaseRange;
            float distance = Vector3.Distance(DetectedPlayer.transform.position, transform.position);
            if (distance <= ChaseRange) return 1f;
            else
            {
                float dist = Vector3.Distance(chaseRangePos, DetectedPlayer.transform.position);
                float ratio = dist / (SearchRange - ChaseRange);
                float lerp = Mathf.Lerp(1, 0, ratio);
                return lerp;
            }
        }
        else
        {
            DetectedPlayer = null;
            return 0f;
        }
    }

    private void OnDie(GameObject deadObject)
    {
        Stop();
        StopAllCoroutines();
        Destroy(EventHandler);
        Animator.CrossFade(ANIM_DEAD, 0.2f);
        StartCoroutine(DieProcess(2f));
    }

    private IEnumerator DieProcess(float disappearTime)
    {
        Collider.enabled = false;
        Rigidbody.useGravity = false;
        float elapsedTime = 0f;
        while (elapsedTime < disappearTime)
        {
            Rigidbody.velocity = Vector3.zero;
            elapsedTime += Time.deltaTime;
            Material.SetFloat("_SplitValue", Mathf.Lerp(4, 0, elapsedTime / disappearTime));
            yield return null;
        }

        Destroy(gameObject);
    }
    private void HandleMove()
    {
        if (KnockbackFeedback.IsKnockback || FreezePosition) return;
        Rigidbody.velocity = _expectedVelocity;
    }

    private void FixedUpdate()
    {
        if (IsStopped) return;
        _stateMachine.FixedUpdate();
    }

    public bool IsEnemyInRange(float range)
    {
        var result = Physics.OverlapSphere(transform.position, range, WhatIsPlayer);
        if (result.Length == 0)
        {
            return false;
        }
        return true;
    }

    public bool IsEnemyInRange(float range, out GameObject enemy)
    {
        var result = Physics.OverlapSphere(transform.position, range, WhatIsPlayer);
        if (result.Length == 0)
        {
            enemy = null;
            return false;
        }
        enemy = result[0].gameObject;
        return true;
    }
    public bool IsEnemyInAttackRange()
    {
        return IsEnemyInRange(AttackRange);
    }

    private void OnDrawGizmos()
    {
        if (RangeDebugging == false) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);      
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, SearchRange);

        // ShoutRange
        DebugAttackRange(Color.green);
        DebugShoutRange(Color.red);
    }

    private void DebugAttackRange(Color color) 
    {
        Gizmos.color = color;
        float radius = Collider.radius * 2f;
        var forward = ModelTransform.forward;
        Vector3 center = transform.position + forward * radius;
        center.y = 0.5f;
        Vector3 right = ModelTransform.right;
        Vector3 left = ModelTransform.right * -1f;
        Vector3 backward = forward * -1f;

        float half = 2f;
        Vector3 btLeft = center + ((left * half) + (backward * half));
        Vector3 btRight = center + ((right * half) + (backward * half));
        Vector3 topLeft = center + ((left * half) + (forward * half));
        Vector3 topRight = center + ((right * half) + (forward * half));
        Gizmos.DrawLine(btLeft, btRight);
        Gizmos.DrawLine(btRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, btLeft);
    }

    private void DebugShoutRange(Color color) 
    {
        Gizmos.color = color;
        float radius = Collider.radius * 2f;
        var forward = ModelTransform.forward;
        Vector3 center = transform.position + forward * (radius * 2f);
        center.y = 0.5f;
        Vector3 right = ModelTransform.right;
        Vector3 left = ModelTransform.right * -1f;
        Vector3 backward = forward * -1f;

        float half = 2f;
        Vector3 btLeft = center + ((left * half) + (backward * half));
        Vector3 btRight = center + ((right * half) + (backward * half));
        Vector3 topLeft = center + ((left * half) + (forward * half));
        Vector3 topRight = center + ((right * half) + (forward * half));
        Gizmos.DrawLine(btLeft, btRight);
        Gizmos.DrawLine(btRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, btLeft);
    }

    public bool ShoutRangeCheck(out Collider hitObject)
    {
        float radius = Collider.radius * 2f;
        var forward = ModelTransform.forward;
        Vector3 forwardPosition = transform.position + forward * (radius * 2f);
        var hit = Physics.OverlapBox(forwardPosition, Vector3.one, ModelTransform.rotation, WhatIsPlayer);
        if(hit.Length != 0)
        {
            hitObject = hit[0];
            return true;
        }

        hitObject = null;
        return false;
    }

    public void Stop()
    {
        IsStopped = true;
        _expectedVelocity = Vector3.zero;
        Rigidbody.velocity = Vector3.zero;
    }

    public void Play()
    {
        IsStopped = false;
    }

    public void FreezeMovement(bool freeze)
    {
        FreezePosition = freeze;
    }
}
