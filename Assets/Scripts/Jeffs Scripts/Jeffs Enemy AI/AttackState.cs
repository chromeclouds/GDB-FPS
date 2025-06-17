using UnityEngine;

public class AttackState : EnemyState
{
    private float attackDuration = 1.2f; //adjust to animation length
    private float timer;
    private int attackCounter = 0;
    private int chosenAttack;

    public AttackState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.animator.SetBool("isAttacking", true);
        ai.PerformMeleeAttack();
    }

    public override void Update()
    {
        if (!ai.CanSeePlayer())
        {
            ai.SwitchState(new SearchState(ai));
        }
        else if (ai.agent.remainingDistance > ai.attackRange)
        {
            ai.SwitchState(new ChaseState(ai));
        }
    }

    public override void Exit()
    {
        ai.animator.SetBool("isAttacking", false);
        
    }
}
