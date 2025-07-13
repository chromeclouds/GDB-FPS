using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SkullEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform gateTarget;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explodeDistance = 1.0f;
    [SerializeField] private int damageToGate = 50;

    private NavMeshAgent agent;
    private bool hasExploded = false;

    public List<LectureEnemyAI> lowlyDemons = new List<LectureEnemyAI>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (gateTarget != null)
        {
            agent.SetDestination(gateTarget.position);
        }
    }

    void Update()
    {
        if (hasExploded || gateTarget == null) return;
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
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void AssignGateTarget(Transform target)
    {
        gateTarget = target;
    }
}