public class PlayerStateFactory : StateFactory<PlayerState>
{
    public PlayerStateFactory(PlayerController pc)
    {
        States = new();
        States.Add(PlayerState.Idle, new PlayerIdleState(pc));
        States.Add(PlayerState.Move, new PlayerMoveState(pc));
        States.Add(PlayerState.Attack, new PlayerAttackState(pc));
        States.Add(PlayerState.Skill, new PlayerSkillState(pc));
    }
}
