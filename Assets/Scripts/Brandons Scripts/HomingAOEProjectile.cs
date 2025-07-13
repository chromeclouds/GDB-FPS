using UnityEngine;
using System.Collections;

public class HomingAOEProjectile : MonoBehaviour, IDamage
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount = 1;
    [SerializeField] float aoeRadius = 5f;
    [SerializeField] LayerMask aoeDamageMask;
    [SerializeField] GameObject explosionVFX;

    [SerializeField] float speed;
    [SerializeField] float destroyTime = 10f;
    [SerializeField] float homingDelay = 2f;
    [SerializeField] float homingSpeed = 8f;
    [SerializeField] float turnRate = 2f;

    bool isHoming = false;
    bool hasExploded = false;

    void Start()
    {
        Destroy(gameObject, destroyTime);

        rb.useGravity = true;
        Vector3 launchDirection = (transform.forward + Vector3.up).normalized;
        rb.AddForce(launchDirection * speed, ForceMode.VelocityChange);

        StartCoroutine(StartHoming());
    }

    void Update()
    {
        if (isHoming && !hasExploded)
        {
            Vector3 toTarget = (gameManager.instance.player.transform.position - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(rb.linearVelocity.normalized, toTarget, turnRate * Time.deltaTime, 0f);
            rb.linearVelocity = newDir * homingSpeed;
        }
    }
    public void takeDamage(int amount)
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        Explode(); // Explodes on any impact (ground, player, wall, etc.)
    }

    void Explode()
    {
        hasExploded = true;

        if (explosionVFX != null)
        {
            GameObject vfxInstance = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            var ps = vfxInstance.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();
            Destroy(vfxInstance, ps.main.duration);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, aoeDamageMask);

        foreach (Collider hit in hits)
        {
            IDamage dmg = hit.GetComponent<IDamage>();
            if (dmg != null)
                dmg.takeDamage(damageAmount);
        }

        Destroy(gameObject);
    }

    IEnumerator StartHoming()
    {
        yield return new WaitForSeconds(homingDelay);
        isHoming = true;
        rb.useGravity = false; // stop gravity once homing starts
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}