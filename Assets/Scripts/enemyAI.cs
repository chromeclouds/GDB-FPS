using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Transform headPos;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] float fleeDistance;    // How close the player must be for the sniper to flee
    [SerializeField] float safeDistance;    // How far sniper must be before resuming attack
    [SerializeField] float fleeSpeed;       // Speed when fleeing

    Color colorOrig;

    float shootTimer;
    float angleToPlayer;

    bool playerInRange;

    bool isFleeing;         // Tracks if sniper is fleeing

    Vector3 playerDir;

    Vector3 fleeDir;        // Sets direction of where the sniper flees
    Vector3 fleeTarget;     // Sets the where the sniper flees to

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);

        // Check if this enemy is a sniper
        if (CompareTag("Sniper"))
        {
            // Start the fleeing behavior loop
            StartCoroutine(fleeCheck());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {

        }
        
    }

    bool canSeePlayer()
    {
        if (isFleeing) return false; // Don't attack while fleeing

        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (angleToPlayer < FOV && hit.collider.CompareTag("Player"))
            {
                shootTimer += Time.deltaTime;
                agent.SetDestination(gameManager.instance.player.transform.position);


                if (shootTimer > shootRate)
                {
                    shoot();
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true;
            }
        }
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void shoot()
    {
        shootTimer = 0;


        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    // Checks if enemy should run away little bit at a time, not all the time
    IEnumerator fleeCheck()
    {
        // Coroutine runs in the background continuously
        while (true)
        {
            // Only run fleeing behavior if player is in range and this enemy is a Sniper
            if (playerInRange && CompareTag("Sniper"))
            {
                // Measure how far the player is from the enemy
                float distance = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
                // If not already fleeing and the player is too close, start fleeing
                if (!isFleeing && distance < fleeDistance)
                {
                    // Mark that the enemy is now fleeing
                    isFleeing = true;
                    // Change movement speed to fleeing speed
                    agent.speed = fleeSpeed;
                }
                // If already fleeing and the player is far enough away, stop fleeing
                else if (isFleeing && distance > safeDistance)
                {
                    // Mark that the enemy is no longer fleeing
                    isFleeing = false;
                }
                // If currently fleeing, set a new direction and move away
                if (isFleeing)
                {
                    // Calculate the direction directly away from the player
                    fleeDir = (transform.position - gameManager.instance.player.transform.position).normalized;
                    // Set the target point to move away from the player
                    fleeTarget = transform.position + fleeDir * 10f;
                    // Tell the NavMeshAgent to go to the flee target
                    agent.SetDestination(fleeTarget);
                }
            }

            // Wait a short time before checking again to avoid overloading the game with updates
            yield return new WaitForSeconds(0.2f);
        }
    }
}
