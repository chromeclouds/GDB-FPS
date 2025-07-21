using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using NUnit.Framework;
using System.Collections.Generic;

public class DemonAI : MonoBehaviour, IDamage, IOpen
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPOS;
    [SerializeField] int HP;
    [SerializeField] int factTargetSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int FOV;
    [SerializeField] int scoreValue;
    [SerializeField] Animator anim;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;

    [SerializeField] private GameObject deathVisual;
    [SerializeField] private Transform vfxSpawn;

    Color colorOrig;

    float shootTimer;
    float angleToPlayer;
    float stoppingDistOrig;
    bool playerInRange;

    Vector3 playerDir;

    private List<GameObject> projectiles = new List<GameObject>();
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
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
        setAnimations();

        if (playerInRange)
        {
            canSeePlayer();
        }
    }

    public void kill()
    {
        Destroy(gameObject);
    }

    void setAnimations()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animSpeedTrans));
    }

    bool canSeePlayer()
    {
        if (!playerInRange) return false;

        playerDir = gameManager.instance.player.transform.position - headPOS.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPOS.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPOS.position, playerDir, out hit))
        {
            if (angleToPlayer < FOV && hit.collider.CompareTag("Player"))
            {
                faceTarget(); // Always rotate toward player

                shootTimer += Time.deltaTime;

                if (shootTimer > shootRate)
                {
                    shoot();
                }

                return true;
            }
        }
        
        return false;
    }

    void faceTarget()
    {
        Vector3 direction = new Vector3(playerDir.x, 0, playerDir.z).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
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
        //
        if (!playerInRange) return;

        shootTimer = 0;
        anim.SetTrigger("Shoot");
    }
    public void endRound()
    {
        gameManager.instance.reduceWallet(scoreValue);
        Destroy(gameObject);
    }

    public void createBullet()
    {
        GameObject newProj = Instantiate(bullet, shootPos.position, transform.rotation);
        projectiles.Add(newProj);
    }
}