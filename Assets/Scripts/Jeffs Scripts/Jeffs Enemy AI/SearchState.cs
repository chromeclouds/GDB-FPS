using UnityEngine;

public class SearchState : EnemyState
{
    private float searchDuration = 5f;
    private float searchTimer = 0f;
    private int idleIndex;

    public SearchState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.agent.SetDestination(ai.lastKnownPosition);

        idleIndex = Random.Range(1, 3); //idle1 or idle2
        ai.animator.SetInteger("IdleIndex", idleIndex);
        ai.animator.SetTrigger("RandomIndex");
    }

    public override void Update()
    {
        searchTimer += Time.deltaTime;

        if (ai.CanSeePlayer())
        {
            ai.SwitchState(new ChaseState(ai));
        }
        else if (searchTimer >= searchDuration)
        {
            ai.SwitchState(new PatrolState(ai));
        }
    }
    
    public override void Exit()
    {
        ai.animator.ResetTrigger("RandomIndex");
    }


}
