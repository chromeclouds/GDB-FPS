using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class SkullEnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] private Transform gateTarget;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explodeDistance = 1.0f;
    [SerializeField] private int damageToGate = 50;

    [SerializeField] private int maxHealth;
    [SerializeField] private Renderer model;

    private NavMeshAgent agent;
    private bool hasExploded = false;
    private int currentHealth;

    private Color colorOrig;

    public List<LectureEnemyAI> lowlyDemons = new List<LectureEnemyAI>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;

        if (model != null)
        {
            colorOrig = model.material.color;
        }

        if (gateTarget != null)
        {
            agent.SetDestination(gateTarget.position);
        }
    }

    void Update()
    {
        if (hasExploded || gateTarget == null) return;
    }

    public void kill()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        Gate gate = other.GetComponentInParent<Gate>();
        if (gate != null)
        {
            gate.TakeDamage(damageToGate);
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        // Destroy all linked lowly demons
        foreach (LectureEnemyAI demon in lowlyDemons)
        {
            if (demon != null)
            {
                Destroy(demon.gameObject);
            }
        }

        if (explosionEffect != null)
        {
            GameObject vfx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f); // Clean up explosion effect after playing
        }

        Destroy(gameObject);
    }

    public void AssignGateTarget(Transform target)
    {
        gateTarget = target;
    }

    public void takeDamage(int amount)
    {
        if (hasExploded) return;

        currentHealth -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (currentHealth <= 0)
        {
            Explode();
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
}