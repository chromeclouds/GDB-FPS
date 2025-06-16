using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.ComponentModel;
using UnityEditor.PackageManager.Requests;

public class enemyAI1 : MonoBehaviour, IDamage
{
    [Header("AI Components")]
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public Transform headPos;
    public Transform player;
    public Animator animator;
   

    [Header("Stats")]
    public int HP;
    public int FOV = 90;
    public float faceTargetSpeed = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int meleeDamage = 10;
    public float attackTimer;
    private int attackCount = 0;

    [HideInInspector] public Vector3 lastKnownPosition;

    private EnemyState currentState;
    private SkinnedMeshRenderer[] renderers;
    private Color[] originalColors;
    

    public void SwitchState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void Start()
    {
        //grab all skinned mesh renderers 
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }
        animator.SetInteger("IdleIndex", 3);
        gameManager.instance.updateGameGoal(1);
        SwitchState(new PatrolState(this));
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        currentState?.Update();
    }

    public bool CanSeePlayer()
    {
        Vector3 dir = player.position - headPos.position;
        float angle = Vector3.Angle(dir, transform.forward);

        if (angle < FOV)
        {
            RaycastHit hit;
            if (Physics.Raycast(headPos.position, dir, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }
        }
        return false;
    }

    public void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            animator.SetBool("isDead", true);
            Destroy(gameObject, 3f);
            gameManager.instance.updateGameGoal(-1);
        }
        else
        {
            animator.SetBool("isDamaged", true);

            animator.SetInteger("TakingDamageType", amount >= 5 ? 2 : 1);
            StartCoroutine(ResetDamageAnimation());

            lastKnownPosition = player.position;
            SwitchState(new ChaseState(this));
        }
    }

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
            animator.SetBool("isAttacking", true);
            animator.SetInteger("AttackIndex", Random.Range(1, 5) == 1 ? 2 : 1); //20% chance of heavy attack
            StartCoroutine(ResetAttack());
        }
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
}