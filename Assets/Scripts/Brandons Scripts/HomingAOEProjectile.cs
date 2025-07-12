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

    //void OnTriggerEnter(Collider other)
    //{
    //    if (hasExploded) return;

    //    // Explode if shot
    //    if (other.CompareTag("PlayerProjectile"))
    //    {
    //        Explode();
    //        return;
    //    }

    //    // Explode if it hits the ground/floor
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        Explode();
    //        return;
    //    }

    //    // Explode if it hits the player, but only if it's not grazing their upper collider
    //    if (other.CompareTag("Player"))
    //    {
    //        float verticalDistance = transform.position.y - other.bounds.center.y;
    //        if (verticalDistance < 1.0f) // adjust as needed
    //        {
    //            Explode();
    //            return;
    //        }
    //    }

    //    // Fallback explode on anything solid (non-trigger)
    //    if (!other.isTrigger)
    //    {
    //        Explode();
    //    }
    //}
    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        Explode(); // Explodes on any impact (ground, player, wall, etc.)
    }

    void Explode()
    {
        hasExploded = true;

        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

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