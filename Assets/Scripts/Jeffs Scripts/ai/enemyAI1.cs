using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.ComponentModel;

public class enemyAI1 : MonoBehaviour, IDamage
{ 
    [Header("Navigation")]
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public Transform player;
    public float lookRadius = 15f;
    public float attackRange = 3f;

    [Header("Health")]
    public int maxHP = 100;
    public int currentHP;
    private bool isDead = false;
    private bool isRaging = false;
    public float rageThreshold = 25f;

    [Header("Combat")]
    public float attackCooldown = 2f;
    [HideInInspector] public float attackTimer = 0f;
    [HideInInspector] public Vector3 lastKnownPosition;

    [Header("Animation")]
    public Animator animator;
    public HammerHitbox hammer;
    public GameObject rangedAttackPrefab;
    public Transform rangedAttackSpawn;

    private EnemyState currentState;
    
    void Start()
    {
        currentHP = maxHP;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SwitchState(new PatrolState(this));
    }

    void Update()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        animator.SetFloat("MoveX", localVelocity.x);
        animator.SetFloat("MoveY", localVelocity.z);
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        if (isDead) return;
        currentState?.Update();
    }

    public void SwitchState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= lookRadius;
    }


    public void FacePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void takeDamage(int amount)
    {
        if (isDead) return;
        currentHP -= amount;

        //stagger
        animator.SetTrigger("isDamaged");
        animator.SetInteger("DamageIndex", amount >= 15 ? 2 : 1);

        if(currentHP <= rageThreshold && !isRaging)
        {
            isRaging = true;
            agent.speed *= 1.5f; //increase speed
            Debug.Log("boss is enraged");

        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetBool("isDead", true);

    }

    public void ResetHit()
    {
        hammer.ResetHit();
    }
    public void EnableHammerDamage()
    {
        hammer.GetComponent<Collider>().enabled = true;
    }
    public void SpawnVerticalSlash()
    {
        Instantiate(rangedAttackPrefab, rangedAttackSpawn.position, rangedAttackSpawn.rotation);
    }

    //used to randomly choose attack
    public int GetBestAttackIndex()
    {
        float yDiff = player.position.y - transform.position.y;
        if (yDiff > 1.5f)
            return 3; //high attack3
        if (Random.value > 0.5f)
            return 1; //side swing attack1
        else
            return 2; //ranged upward attack2
    }


}

//old code from old broken attempt at the boss
/*
    IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isDamaged", false);
    }

    IEnumerator FlashRed()
    {
        foreach (var r in renderers)
            r.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }
    
    public void Attack()
    {
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            int attackIndex = (Random.Range(1, 6) == 5) ? 2 : 1; //5th attack is heavy
            
            animator.SetInteger("AttackIndex", attackIndex);
            animator.SetBool("isAttacking", true);
            
        }
    }

    public void EndAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void PlayDamageReaction(bool isShotgun)
    {
        if (isShotgun)
        {
            animator.SetInteger("TakingDamageType", 2);
            animator.SetBool("isShotgunHit", true);

        }
        else
        {
            animator.SetInteger("TakingDamageType", 1);
            animator.SetBool("isDamaged", true);
        }
    }

    public void PerformMeleeAttack()
    {
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            animator.SetBool("isAttacking", true);
            int attackType = Random.Range(1, 6);
            animator.SetInteger("Attackindex", attackType == 5 ? 2 : 1);

            StartCoroutine(HandleMeleeDamage(attackType == 5));
        }
    }

    private IEnumerator HandleMeleeDamage(bool isPowerHit)
    {
        //wait for hit frame, adjust to match anim here
        yield return new WaitForSeconds(0.5f);

        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 1.5f, 2f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                int damage = isPowerHit ? meleeDamage * 2 : meleeDamage;
                hit.GetComponent<IDamage>()?.takeDamage(damage);
                if (isPowerHit)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.AddForce((hit.transform.position - transform.position).normalized *500f);
                }
            }
        }
        //wait for attack to finish
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttacking", false);
    }
    public void EndDamageReaction()
    {
        animator.SetBool("isShotgunHit", false);
        animator.SetBool("isDamaged", false);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.2f); //adjust this to match animation lengths
        animator.SetBool("isAttacking", false);
    }

    public void SetMovementAnimation(bool isWalking, bool isRunning)
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
    }

    public void SetRandomIdle()
    {
        int idleChoice = Random.Range(0, 2); //0 idle1, 1 idle2
        animator.SetInteger("IdleIndex", idleChoice);
    }

    public void DealMeleeDamage()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position + transform.forward * 1.5f, 1f);
        foreach (var hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
            {
                IDamage dmg = hit.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(meleeDamage);
                }
            }
        }
    }
    */
