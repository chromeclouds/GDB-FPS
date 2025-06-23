using UnityEngine;

public class DemonFlameBullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifeTime = 5f;
    
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamage damageable = other.GetComponent<IDamage>();
            if(damageable != null )
            {
                damageable.takeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

}
