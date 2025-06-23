using UnityEngine;

public class ChaseState : EnemyState
{
     

    public ChaseState(enemyAI1 ai) : base(ai) { }
    
    public override void Enter()
    {
        ai.animator.SetBool("isChasing", true);
        ai.animator.SetBool("isRunning", true);
    }

    public override void Update()
    {
        ai.attackTimer += Time.deltaTime;

        if (ai.CanSeePlayer())
        {
            ai.agent.SetDestination(ai.player.position);
            ai.lastKnownPosition = ai.player.position;

            
            if (ai.agent.remainingDistance <= ai.attackRange)
            {
                ai.FacePlayer();
                ai.SwitchState(new AttackState(ai));
            }
        }
        else
        {
            ai.SwitchState(new SearchState(ai));
        }
    }

    public override void Exit()
    {
        ai.animator.SetBool("isChasing", false);
        ai.animator.SetBool("isRunning", false);
    }
}
