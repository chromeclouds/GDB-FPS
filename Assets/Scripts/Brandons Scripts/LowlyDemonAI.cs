using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
public class LowlyDemonAI : MonoBehaviour, IDamage, IOpen
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPOS;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDist;
    [SerializeField] int roamstopTime;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int FOV;
    [SerializeField] int scoreValue;
    [SerializeField] Animator anim;

    [SerializeField] public Transform skullTarget;
    [SerializeField] private Vector3 followOffset = Vector3.zero;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private GameObject deathVisual;
    [SerializeField] private Transform vfxSpawn;

    Color colorOrig;

    float shootTimer;
    float angleToPlayer;
    float roamTime;
    float stoppingDistOrig;
    bool playerInRange;

    bool isFollowingSkull => skullTarget != null;

    Vector3 playerDir;
    Vector3 startingPos;

    private List<GameObject> projectiles = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;

        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        if (idleSound != null && audioSource != null)
        {
            audioSource.clip = idleSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        setAnimations();

        if (agent.remainingDistance < 0.01f)
        {
            roamTime += Time.deltaTime;

        }
        if (playerInRange && canSeePlayer())
        {
            return;
        }
        if (playerInRange && !canSeePlayer())
        {
            roamCheck();
            return;
        }
        if (!playerInRange && isFollowingSkull)
        {
            FollowSkull();
            return;
        }
        roamCheck();
    }

    public void SetFollowOffset(Vector3 offset)
    {
        followOffset = offset;
    }

    void FollowSkull()
    {
        if (skullTarget == null) return;

        Vector3 targetPos = skullTarget.position + followOffset;
        agent.SetDestination(targetPos);
        faceTarget(targetPos);
    }
    void faceTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    void setAnimations()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animSpeedTrans));
    }

    void roamCheck()
    {
        if (roamTime >= roamstopTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTime = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        bool found = NavMesh.SamplePosition(ranPos, out hit, roamDist, NavMesh.AllAreas); // safer mask

        if (found && hit.position != Vector3.positiveInfinity)
        {
            agent.SetDestination(hit.position);
        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPOS.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPOS.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPOS.position, playerDir, out hit))
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
                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);//Change direction over time
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
            agent.stoppingDistance = 0;
        }
    }
    public void takeDamage(int amount)
    {
        if (HP <= 0 || agent == null || !agent.isOnNavMesh) return;
        HP -= amount;
        if (HP > 0)
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashRed());
        }
        else
        {
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.increaseWallet(scoreValue);
            StartCoroutine(DeathSequence());
        }
        /*
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.increaseWallet(scoreValue);
        }
        else
        {
            StartCoroutine(flashRed());
        }
        */
    }

    public void kill()
    {
        Destroy(gameObject);
    }

    public void endRound()
    {
        gameManager.instance.reduceWallet(scoreValue);
        Destroy(gameObject);
    }

    public IEnumerator SafeDeath()
    {
        if (model != null) model.enabled = false;
        if (agent != null) agent.enabled = false;
        yield return new WaitForSeconds(0.25f); // Let logic settle
        Destroy(gameObject);
    }

    IEnumerator DeathSequence()
    {
        foreach (GameObject proj in projectiles)
        {
            if (proj != null)
                Destroy(proj);
        }
        if (deathVisual != null)
        {
            GameObject vfx = Instantiate(deathVisual, vfxSpawn.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }
        if (model != null)
        {
            model.enabled = false;
        }
        if (agent != null)
        {
            agent.enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator flashRed() //Timer
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    //Alternative colors
    IEnumerator flashOrange() //Timer
    {
        model.material.color = Color.orange;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    IEnumerator flashYellow() //Timer
    {
        model.material.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void shoot()
    {
        shootTimer = 0;
        anim.SetTrigger("Shoot");
    }

    public void createBullet()
    {
        GameObject newProj = Instantiate(bullet, shootPos.position, transform.rotation);
        projectiles.Add(newProj);

    }
}