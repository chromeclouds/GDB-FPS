using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class turretPlayer : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public float attackTimer;
    private int attackCount;
    public float attackCooldown = 1.5f;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] private float faceTargetSpeed = 5f;

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurretEnemy"))
        {
            Debug.Log("Enemy entered detection range: " + other.name);
            faceTarget(other.transform);
            Attack();
            StartCoroutine(AttackCooldown());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TurretEnemy"))
        {
            faceTarget(other.transform);
            Attack();
            StartCoroutine(AttackCooldown());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TurretEnemy"))
        {
            Debug.Log("Enemy exited detection range: " + other.name);

        }
    }
    private IEnumerator AttackCooldown()
    {

        yield return new WaitForSeconds(attackCooldown);

    }
    public void Attack()
    {
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            int attackIndex = (Random.Range(1, 6) == 5) ? 2 : 1; 
            createBullet();
        }
    }
    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    void faceTarget(Transform enemy)
    {
        Vector3 direction = enemy.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

}
