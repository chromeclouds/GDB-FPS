using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private int currentWaypoint = 0;

    private float wanderRadius = 25f;
    private Vector3 wanderTarget;
    private float waitDuration = 5f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public PatrolState(enemyAI1 ai) : base(ai) { }

    public override void Enter()
    {
        ai.animator.SetBool("isPatrolling", true);
        ai.animator.SetBool("isWalking", true);

        if (ai.waypoints == null || ai.waypoints.Length == 0)
        {
            wanderTarget = RandomNavSphere(ai.transform.position, wanderRadius, -1);
            ai.agent.SetDestination(wanderTarget);
            isWaiting = false;
            waitTimer = 0f;
        }
        else
        {
            currentWaypoint = (currentWaypoint + 1) % ai.waypoints.Length;
            ai.agent.SetDestination(ai.waypoints[currentWaypoint].position);
        }
    }

    public override void Update()
    {
        Vector3 localVelocity = ai.transform.InverseTransformDirection(ai.agent.velocity);
        ai.animator.SetFloat("MoveX", localVelocity.x);
        ai.animator.SetFloat("MoveY", localVelocity.z);
        ai.animator.SetFloat("MoveSpeed", ai.agent.velocity.magnitude);

        if (ai.CanSeePlayer())
        {
            ai.SwitchState(new ChaseState(ai));
            return;
        }

        if (!ai.agent.pathPending && ai.agent.remainingDistance < 0.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = 0f;
                ai.animator.SetBool("isWalking", false);
            }
            else
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitDuration)
                {
                    Enter(); //pick new wander point
                }
            }
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public override void Exit()
    {
        ai.animator.SetBool("isPatrolling", false);
        ai.animator.SetBool("isWalking", false);
    }
}