using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class SkullEnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] private Transform gateTarget;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explodeDistance;
    [SerializeField] private int damageToGate;

    [SerializeField] private int maxHealth;
    [SerializeField] private Renderer model;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip explodeSound;


    private NavMeshAgent agent;
    private bool hasExploded = false;
    private int currentHealth;

    private Color colorOrig;

    public List<LowlyDemonAI> lowlyDemons = new List<LowlyDemonAI>();

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
        if (idleSound != null && audioSource != null)
        {
            audioSource.clip = idleSound;
            audioSource.loop = true;
            audioSource.Play();
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
        foreach (LowlyDemonAI demon in lowlyDemons)
        {
            if (demon != null)
            {
                demon.StartCoroutine(demon.SafeDeath());
            }
        }

        if (explosionEffect != null)
        {
            GameObject vfx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f); // Clean up explosion effect after playing
        }
        if (explodeSound != null)
        {
            GameObject tempGO = new GameObject("TempExplosionSound");
            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = explodeSound;
            aSource.spatialBlend = 0f; // 0 = 2D, no spatial falloff
            aSource.volume = 2.0f;
            aSource.Play();
            Destroy(tempGO, explodeSound.length);
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

        if (currentHealth <= 0)
        {
            Explode();
        }
        //else
        //{
        //    StartCoroutine(flashRed());
        //}
    }
    //IEnumerator flashRed()
    //{
    //    model.material.color = Color.red;
    //    yield return new WaitForSeconds(0.1f);
    //    model.material.color = colorOrig;
    //}
}