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
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayerMask) != 0)
        {
            Debug.Log("Enemy entered detection range: " + other.name);
            createBullet();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TurretEnemy"))
        {
            Debug.Log("Enemy exited detection range: " + other.name);
            // Optional: Remove target or stop firing
        }
    }
    public void Attack()
    {
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            int attackIndex = (Random.Range(1, 6) == 5) ? 2 : 1; //5th attack is heavy



        }
    }
    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);

    }
}
