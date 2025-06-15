using UnityEngine;

public class PatrolState : EnemyState
{
    private int currentWaypoint = 0;
    public PatrolState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.animator.SetBool("isWalking", true);

        if (ai.waypoints.Length > 0)
        {
            ai.agent.SetDestination(ai.waypoints[currentWaypoint].position);
        }
    }

    public override void Update()
    {
        if (ai.CanSeePlayer())
        {
            ai.SwitchState(new ChaseState(ai));
            return;
        }

        if (!ai.agent.pathPending && ai.agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % ai.waypoints.Length;
            ai.agent.SetDestination(ai.waypoints[currentWaypoint].position);
        }
    }

    public override void Exit()
    {
        ai.animator.SetBool("isWalking", false);
    }
}