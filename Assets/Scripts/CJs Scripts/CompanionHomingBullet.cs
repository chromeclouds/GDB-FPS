using UnityEngine;

public class CompanionHomingBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 aimPoint = target.position + Vector3.up * 1.5f;
        Vector3 direction = (aimPoint - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        IDamage dmg = collision.collider.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }
        Destroy(gameObject);
    }


}
