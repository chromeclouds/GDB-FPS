using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int explosionDamage = 100;
    public GameObject explosionEffectPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 5f); //make sure explosive bullets arent left around
                                 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        if(explosionEffectPrefab)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            IDamage dmg = nearby.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(explosionDamage);
            }
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
