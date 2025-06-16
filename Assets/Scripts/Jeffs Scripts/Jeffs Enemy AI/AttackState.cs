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
        timer = 0f;
        ai.agent.ResetPath(); //stop moving for attack
        attackCounter++;
        chosenAttack = (attackCounter % 5 == 0) ? 2 : 1; //attack 2 every 5 hits
        ai.animator.SetBool("isAttacking", true);
        ai.animator.SetInteger("AttackIndex", chosenAttack);

        
        ai.FacePlayer();
        ai.Attack(); 
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        ai.FacePlayer();

        if (timer >= attackDuration)
        {
            if (ai.CanSeePlayer() && ai.agent.remainingDistance > ai.attackRange + 0.5f)
            {
                ai.SwitchState(new ChaseState(ai));
            }
            else if (!ai.CanSeePlayer())
            {
                ai.SwitchState(new SearchState(ai));
            }
            else
            {
                //reset attack if in range still
                ai.SwitchState(new AttackState(ai));
            }
        }
    }

    public override void Exit()
    {
        ai.animator.SetBool("isAttacking", false);
        ai.animator.SetInteger("AttackIndex", 0);
    }
}
