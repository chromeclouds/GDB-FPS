using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour, IOpen
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    public float atkRange;
    public float atkCD;
    public LayerMask enemyLayer;

    Vector3 dest;

    private float atkTimer;

    // Update is called once per frame
    void Update()
    {
        dest = player.position;
        agent.destination = dest;

        atkTimer -= Time.deltaTime;

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, atkRange, enemyLayer);
        if (enemiesInRange.Length > 0)
        {
            Transform closestEnemy = GetClosestEnemy(enemiesInRange);
            if (closestEnemy != null)
            {
                FaceTarget(closestEnemy);
                if (atkTimer <= 0f)
                {
                    shoot(closestEnemy.position, closestEnemy);
                    atkTimer = atkCD;
                }
            }
        }
    }

    Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform closest = null;
        float closestDist = Mathf.Infinity;
        foreach (Collider enemyCollider in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemyCollider.transform;
            }
        }
        return closest;
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void shoot(Vector3 targetPos, Transform target)
    {
        Vector3 targetAimPoint = targetPos + Vector3.up * 1.5f;

        Vector3 direction = (targetAimPoint - shootPos.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject newBullet = Instantiate(bullet, shootPos.position, rotation);

        CompanionHomingBullet bulletScript = newBullet.GetComponent<CompanionHomingBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
    }
}
