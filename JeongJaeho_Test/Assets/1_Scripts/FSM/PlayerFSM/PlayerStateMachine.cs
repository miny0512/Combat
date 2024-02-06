public enum PlayerState
{
    Idle,
    Move,
    Attack,
    Skill,
}
public class PlayerStateMachine : StateMachine<PlayerState, PlayerController>
{
    public PlayerStateMachine(PlayerController pc) : base(pc)
    {
    }

    protected override void Init(PlayerController owner)
    {
        this.owner = owner;
        factory = new PlayerStateFactory(owner);
        InitTransitions();
        SetDefaultState();
    }

    protected override void InitTransitions()
    {
        // Idle -> ~
        AddTransition(factory[PlayerState.Idle], factory[PlayerState.Move], ()=> owner.IsMovementPressed && !owner.IsAttackButtonPressed);
        AddTransition(factory[PlayerState.Idle], factory[PlayerState.Attack], () => owner.IsAttackButtonPressed);
        AddTransition(factory[PlayerState.Idle], factory[PlayerState.Skill], () => owner.IsActiveSkill);

        // Move -> ~
        AddTransition(factory[PlayerState.Move], factory[PlayerState.Attack], ()=> owner.IsAttackButtonPressed);
        AddTransition(factory[PlayerState.Move], factory[PlayerState.Idle], ()=> !owner.IsMovementPressed && !owner.IsAttackButtonPressed);
        AddTransition(factory[PlayerState.Move], factory[PlayerState.Skill], () => owner.IsActiveSkill);

        // Attack -> ~
        AddTransition(factory[PlayerState.Attack], factory[PlayerState.Idle], ()=> owner.IsAttackEnd);
        AddTransition(factory[PlayerState.Attack], factory[PlayerState.Skill], ()=> owner.IsActiveSkill);

        // Skill -> ~
        AddTransition(factory[PlayerState.Skill], factory[PlayerState.Idle], ()=> owner.IsActiveSkill == false && owner.CurrentSkill == null);
        AddTransition(factory[PlayerState.Skill], factory[PlayerState.Idle], ()=> owner.IsStopped);
    }

    protected override void SetDefaultState()
    {
        ChangeState(factory[PlayerState.Idle]);
    }

}
