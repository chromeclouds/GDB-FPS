using UnityEngine;

public class AttackState : EnemyState
{
    private int attackCounter = 0;

    public AttackState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.agent.isStopped = true;
        ai.FacePlayer();
    }

    public override void Update()
    {
        ai.FacePlayer();

        if (!ai.CanSeePlayer())
        {
            ai.agent.isStopped = false;
            ai.SwitchState(new SearchState(ai));
            return;
        }
        if (ai.agent.remainingDistance > ai.attackRange)
        {
            ai.agent.isStopped = false;
            ai.SwitchState(new ChaseState(ai));
            return;
        }
        if (ai.attackTimer >= ai.attackCooldown)
        {
            ai.attackTimer = 0f;
            attackCounter++;
            if (attackCounter >= 5)
            {
                ai.animator.SetTrigger("Attack2");
                attackCounter = 0;
                //heavy damage and knockback eventually
            }
            else
            {
                ai.animator.SetTrigger("Attack1");
                //normal damage here
            }
        }
    }
    public override void Exit()
    {
        ai.agent.isStopped = false;
    }
}
