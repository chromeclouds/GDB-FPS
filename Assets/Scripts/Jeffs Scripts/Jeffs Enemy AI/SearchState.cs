using UnityEngine;

public class SearchState : EnemyState
{
    private float searchDuration = 3f;
    private float searchTimer = 0f;
    private int randomIdleIndex;

    public SearchState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.agent.SetDestination(ai.lastKnownPosition);
        randomIdleIndex = Random.Range(1, 3); //idle1 or idle2
        ai.animator.SetTrigger($"Idle{randomIdleIndex}");
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
        ai.animator.ResetTrigger($"Idle{randomIdleIndex}");
    }


}
