public class EnemyStateFactory : StateFactory<EnemyState> 
{
    public EnemyStateFactory(EnemyAIController aicontroller)
    {
        States = new();
        States.Add(EnemyState.Idle, new EnemyIdleState(aicontroller));
        States.Add(EnemyState.Attack, new EnemyAttackState(aicontroller));
        States.Add(EnemyState.Chase, new EnemyChaseState(aicontroller));
        States.Add(EnemyState.Shout, new EnemyShoutState(aicontroller));
        States.Add(EnemyState.Return, new EnemyReturnState(aicontroller));
    }
}
