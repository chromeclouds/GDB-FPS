using UnityEngine;

public class AttackState : EnemyState
{
    private float attackDuration = 1.2f; //adjust to animation length
    private float timer =0f;
    private int chosenAttack;

    public AttackState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        timer = 0f;

        ai.agent.isStopped = true;

        chosenAttack = ai.GetBestAttackIndex();

        ai.animator.SetBool("isAttacking", true);
        ai.animator.SetInteger("AttackIndex", chosenAttack);
        ai.FacePlayer();

        ai.animator.SetTrigger("AttackTrigger");
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackDuration)
        {
            ai.attackTimer = 0f;
            float distance = Vector3.Distance(ai.transform.position, ai.player.position);
            if (!ai.CanSeePlayer())
            {
                ai.agent.isStopped = false;
                ai.SwitchState(new SearchState(ai));
            }
            else if (distance > ai.attackRange)
            {
                ai.agent.isStopped = false;
                //choose to return to rage if raging else chase
                if (ai.currentHP <= ai.rageThreshold)
                    ai.SwitchState(new RageState(ai));
                else
                    ai.SwitchState(new ChaseState(ai));
            } else
            {
                ai.SwitchState(new AttackState(ai)); //continue attacking
            }
            }
        }

    public override void Exit()
    {
        ai.animator.SetBool("isAttacking", false);
        ai.animator.SetInteger("AttackIndex", 0);
        ai.animator.ResetTrigger("AttackTrigger");
        ai.agent.isStopped = false; 

    }
}
