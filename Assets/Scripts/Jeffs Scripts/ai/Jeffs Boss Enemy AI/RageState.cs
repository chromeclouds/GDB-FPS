using UnityEngine;

public class RageState : EnemyState
{
    public RageState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.animator.SetBool("isChasing", true);
        ai.animator.SetBool("isRunning", true);
        ai.agent.speed *= 1.25f; //speed boost
        Debug.Log("Boss Entering Rage State");
    }

    
    public override void Update()
    {
        ai.attackTimer += Time.deltaTime;

        Vector3 localVelocity = ai.transform.InverseTransformDirection(ai.agent.velocity);
        ai.animator.SetFloat("MoveX", localVelocity.x);
        ai.animator.SetFloat("MoveY", localVelocity.z);

        if (ai.player == null) return;
        ai.agent.SetDestination(ai.player.position);
        ai.FacePlayer();

        float distance = Vector3.Distance(ai.transform.position, ai.player.position);
        if (distance <= ai.attackRange)
        {
            ai.SwitchState(new AttackState(ai));
        }
    }

    public override void Exit()
    {
        ai.animator.SetBool("isChasing", false);
        ai.animator.SetBool("isRunning", false);
    }
}
