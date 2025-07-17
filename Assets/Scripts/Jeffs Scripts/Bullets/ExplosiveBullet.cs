using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 100; //now gets damage from weaponfire then weapondata
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
                dmg.takeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    
}
