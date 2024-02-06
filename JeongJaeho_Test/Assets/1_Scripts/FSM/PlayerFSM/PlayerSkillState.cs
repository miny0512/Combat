public class PlayerSkillState : PlayerState<PlayerController>
{
    public PlayerSkillState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        owner.CurrentSkill?.SkillEnter();
    }

    public override void Exit()
    {
        owner.CurrentSkill?.SkillExit();
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        owner.CurrentSkill?.Activate();
    }
}
