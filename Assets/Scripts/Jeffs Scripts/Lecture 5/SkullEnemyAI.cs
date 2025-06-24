using System.Collections;
using UnityEngine;

public class SkullEnemyAI : MonoBehaviour, IDamage
{
    [Header("Settings")]
    public bool isPatrolling = true;
    public float speed = 3f;
    public float health = 50f;
    public float fireRate = 2f;
    public float shootRange = 12f;
    public float fieldOfViewAngle = 100f;
    public int bulletDamage = 10;

    private float floatAmplitude = 0.5f;
    private float floatFrequency = 1f;
    private Vector3 startPosition;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    [Header("Wander")]
    public float wanderRadius = 3f;
    public float wanderInterval = 3f;
    private Vector3 wanderTarget;
    private float wanderTimer;

    [Header("Combat")]
    public GameObject eyeBulletPrefab;
    public Transform shootPoint;

    private float shootTimer;
    private bool isDead = false;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentPatrolIndex = 0;
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDead || player == null) return;

        //apply floating motion using sine wave
        Vector3 newPos = startPosition;
        newPos.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newPos.y, transform.position.z);


        shootTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            ChasePlayer();
            TryShoot();
        }
        else if (isPatrolling && patrolPoints.Length >= 2)
        {
            Patrol();
        }
        else
        {
            Wander();
        }
    }

    private void Patrol()
    {
        Transform target = patrolPoints[currentPatrolIndex];
        MoveAndRotateTo(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval || Vector3.Distance(transform.position, wanderTarget) < 0.5f)
        {
            Vector3 offset = Random.insideUnitSphere * wanderRadius;
            offset.y = 0;
            wanderTarget = transform.position + offset;
            wanderTimer = 0f;
        }

        MoveAndRotateTo(wanderTarget, speed * 0.5f);
    }

    private void ChasePlayer()
    {
        MoveAndRotateTo(player.position, speed * 0.75f);
    }

    private void TryShoot()
    {
        if (shootTimer >= fireRate && Vector3.Distance(transform.position, player.position) <= shootRange)
        {
            shootTimer = 0f;

            GameObject bullet = Instantiate(eyeBulletPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 dir = (player.position - shootPoint.position).normalized;
                rb.linearVelocity = dir * 10f;
            }

            DemonFlameBullet bulletScript = bullet.GetComponent<DemonFlameBullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = bulletDamage;
            }
        }
    }

    private bool PlayerInSight()
    {
        Vector3 toPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, toPlayer);

        if (angle < fieldOfViewAngle * 0.5f && Vector3.Distance(transform.position, player.position) <= shootRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, toPlayer.normalized, out hit, shootRange))
            {
                return hit.transform.CompareTag("Player");
            }
        }

        return false;
    }

    private void MoveAndRotateTo(Vector3 target, float moveSpeed = -1f)
    {
        if (moveSpeed < 0) moveSpeed = speed;

        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion look = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 4f);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    public void takeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
