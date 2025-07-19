using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HellBornDemonAI : MonoBehaviour, IDamage, IOpen
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPOS;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] Animator anim;
    

    [SerializeField] int HP;
    [SerializeField] float shootRate;
    [SerializeField] int factTargetSpeed;
    [SerializeField] float roamDist = 10f;
    [SerializeField] float roamStopTime = 2f;
    [SerializeField] float FOV;
    [SerializeField] int animSpeedTrans = 5;
    [SerializeField] int scoreValue;
    [SerializeField] float idleSoundRate;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip floatingSound;

    [SerializeField] private LayerMask lineOfSightMask;

    Color colorOrig;
    private float shootTimer;
    private float roamTime;
    private float stoppingDistOrig;
    private bool searchingPlayer = false;

    Vector3 playerDir;
    Vector3 lastKnownPlayerPos;

    void Start()
    {
        stoppingDistOrig = agent.stoppingDistance;
        colorOrig = model.material.color;
        roamTime = roamStopTime + 1f; // force roam start

        if (floatingSound != null && audioSource != null)
        {
            audioSource.clip = floatingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        SetAnimations();

        if (agent.remainingDistance < 0.1f)
            roamTime += Time.deltaTime;

        bool seesPlayer = CanSeePlayer();

        if (!seesPlayer)
        {
            if (searchingPlayer && agent.remainingDistance < 0.1f)
            {
                SearchAroundLastSeenLocation();
            }
            else
            {
                RoamCheck();
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.velocity = Vector3.zero;
    }

    public void kill()
    {
        Destroy(gameObject);
    }

    void SetAnimations()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float currentAnimSpeed = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(currentAnimSpeed, agentSpeed, Time.deltaTime * animSpeedTrans));
    }

    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPOS.position;
        float angle = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPOS.position, playerDir, Color.red);

        if (angle < FOV)
        {
            if (Physics.Raycast(headPOS.position, playerDir, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    lastKnownPlayerPos = gameManager.instance.player.transform.position;
                    searchingPlayer = true;

                    float distToPlayer = Vector3.Distance(transform.position, lastKnownPlayerPos);
                    agent.isStopped = false;
                    agent.SetDestination(lastKnownPlayerPos);

                    FacePlayer();

                    shootTimer += Time.deltaTime;
                    if (shootTimer >= shootRate)
                    {
                        shootTimer = 0;
                        anim.SetTrigger("Shoot");
                    }

                    agent.stoppingDistance = stoppingDistOrig;
                    return true;
                }
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    void FacePlayer()
    {
        if (playerDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * factTargetSpeed);
        }
    }

    void RoamCheck()
    {
        if (roamTime >= roamStopTime)
        {
            Roam();
        }
    }

    void Roam()
    {
        roamTime = 0;
        agent.stoppingDistance = 0;

        Vector3 randPos = Random.insideUnitSphere * roamDist * 5 + transform.position; ;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randPos, out hit, roamDist, NavMesh.AllAreas))
        {
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            agent.SetDestination(hit.position + offset);
        }
    }

    void SearchAroundLastSeenLocation()
    {
        roamTime = 0;
        agent.stoppingDistance = 0;

        float radius = roamDist + Random.Range(0f, 5f);
        Vector3 randPos = Random.insideUnitSphere * radius + lastKnownPlayerPos;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randPos, out hit, radius, NavMesh.AllAreas))
        {
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            agent.SetDestination(hit.position + offset);
        }
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(lastKnownPlayerPos);
        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.increaseWallet(scoreValue);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    public void endRound()
    {
        gameManager.instance.reduceWallet(scoreValue);
        Destroy(gameObject);
    }
}