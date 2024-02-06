using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [field: HeaderColor("References", ColorType.WHITE)]
    [field: SerializeField] public CinemachineVirtualCamera FrontViewCamera { get; private set; }
    [field: SerializeField] public Transform ModelTransform { get; private set; }
    [field: SerializeField] public string[] SlashParticleName{ get; private set; }
    [field: SerializeField] public Transform[] SlashParticleTransform{ get; private set; }
    [field: SerializeField, Readonly] public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField, Readonly] public Health Health { get; private set; }
    [field: SerializeField, Readonly] public Animator Animator { get; private set; }
    [field: SerializeField, Readonly] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField, Readonly] public PlayerAnimationEventHandler EventHandler{ get; private set; }
    [field: SerializeField, Readonly] public HitFeedback HitFeedback { get; private set; }
    [field: SerializeField, Readonly] public KnockbackFeedback KnockbackFeedback{ get; private set; }
    [field: Space(20)]

    [field: HeaderColor("Layer", ColorType.WHITE)]
    [field:SerializeField] public LayerMask WhatIsEnemy { get; private set; }

    [field:Space(20)]

    [field:HeaderColor("Stats", ColorType.WHITE)]
    [field: SerializeField] public int AttackPower { get; private set; } = 1;
    [field: SerializeField] public int AttackRange { get; private set; } = 1;
    [field: SerializeField] public MovementStatus MovementStatus { get; private set; }
    [field: SerializeField, Readonly] public bool IsStopped { get; private set; }
    [field: SerializeField, Readonly] public bool IsActiveSkill { get;  set; }
    [field: SerializeField, Readonly] public SkillBase CurrentSkill { get; set; }
    [field: SerializeField, Readonly] public bool FreezePosition{ get; private set; }

    [field: Space(20)]
    [field: HeaderColor("Inputs", ColorType.WHITE)]
    [field: SerializeField, Readonly] public Vector3 InputDirection { get; private set; }
    [field: SerializeField, Readonly] public bool IsMovementPressed { get; private set; }
    [field: SerializeField, Readonly] public bool IsAttackButtonPressed { get; set; }
    public bool IsAttackEnd { get; set; } = true;

    [SerializeField, Readonly] private Vector3 _expectedVelocity;
    public Vector3 ExpectedVelocity { get { return _expectedVelocity; } set { _expectedVelocity = value; } }
    public float ExpectedVelocityX { get { return _expectedVelocity.x; } set { _expectedVelocity.x = value; } }
    public float ExpectedVelocityY { get { return _expectedVelocity.y; } set { _expectedVelocity.y = value; } }
    public float ExpectedVelocityZ { get { return _expectedVelocity.z; } set { _expectedVelocity.z = value; } }


    private PlayerStateMachine _stateMachine;

    // Anim Hashing
    // Param
    public readonly int ANIM_PARAMETER_FORWARDSPEED = Animator.StringToHash("ForwardSpeed");
    // Anim clip
    public readonly int ANIM_VICTORY = Animator.StringToHash("Victory");
    public readonly int ANIM_LOCOMOTION = Animator.StringToHash("Locomotion");
    public readonly int ANIM_SPINATTACK = Animator.StringToHash("SpinAttack");
    public readonly int ANIM_DASHATTACK = Animator.StringToHash("DashAttack");
    public readonly int ANIM_DEAD = Animator.StringToHash("Dead");
    public readonly int[] ANIM_ATTACK = new int[]{
        Animator.StringToHash("Attack_1"),
        Animator.StringToHash("Attack_2"),
        Animator.StringToHash("Attack_3")
    };

    private void Awake()
    {
        KnockbackFeedback= GetComponent<KnockbackFeedback>();
        EventHandler = GetComponent<PlayerAnimationEventHandler>();
        PlayerInput = GetComponent<PlayerInput>();
        MovementStatus = GetComponent<MovementStatus>();
        Health = GetComponent<Health>();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        _stateMachine = new PlayerStateMachine(this);
    }

    private void OnEnable()
    {
        PlayerInput.OnMoveAction -= OnMove;
        PlayerInput.OnMoveAction += OnMove;
        Health.OnDie -= OnDie;
        Health.OnDie += OnDie;


        Health.OnDie -= GameManager.Instance.OnGameSet;
        Health.OnDie += GameManager.Instance.OnGameSet;
    }

    private void OnDisable()
    {
        PlayerInput.OnMoveAction -= OnMove;
        Health.OnDie -= OnDie;


        Health.OnDie -= GameManager.Instance.OnGameSet;
    }

    private void OnDie(GameObject deadObject)
    {
        Animator.CrossFade(ANIM_DEAD, 0.1f);
        Stop();
        Destroy(gameObject, 2f);
    }

    private void OnMove(Vector3 movementInput)
    {
        IsMovementPressed = movementInput != Vector3.zero;
        InputDirection = movementInput;
    }

    private void Update()
    {
        if (IsStopped)
        {
            return;
        }
        _stateMachine?.Update();
        HandleMove();
        HandleRotate();
    }

    private void FixedUpdate()
    {
        if (IsStopped)
        {
            return;
        }
        _stateMachine?.FixedUpdate();
    }

    private void HandleMove()
    {
        if (KnockbackFeedback.IsKnockback || FreezePosition) return;
        Rigidbody.velocity = _expectedVelocity;
    }

    private float _currentVelocity;
    private float _smoothTime = 0.1f;

    private void HandleRotate()
    {
        if (KnockbackFeedback.IsKnockback || FreezePosition) return;

        if(InputDirection == Vector3.zero)
        {
            return;
        }

        var targetAngle = Mathf.Atan2(InputDirection.x, InputDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(ModelTransform.localRotation.eulerAngles.y, targetAngle, ref _currentVelocity, _smoothTime);
        ModelTransform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    public bool HitCheck(out Collider hit)
    {
        var result = Physics.OverlapSphere(transform.position, AttackRange, WhatIsEnemy);
        hit = null;
        if(result.Length != 0)
        {
            hit = result[0];
            return true;
        }
        return false;
    }

    public void Stop()
    {
        IsStopped = true;
        _expectedVelocity = Vector3.zero;
        Rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public void Play()
    {
        IsStopped = false;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void FreezeMovement(bool freeze)
    {
        FreezePosition = freeze;
        if (freeze)
        {
            _expectedVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
        }
    }

}
