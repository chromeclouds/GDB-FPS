using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent (typeof(Rigidbody))]
public class FireBullet : MonoBehaviour
{
    public float stickDuration = 3f;
    public float burnDuration = 2f;
    public WeaponData weaponData;
    public float burnDamage = 5f;
    public ParticleSystem flameEffect;
    public float travelSpeed = 12f;
    public float lifeTime = 1.25f;

    private bool hasStuck = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * travelSpeed;

        Destroy(gameObject, lifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (flameEffect != null && !flameEffect.isPlaying)
            flameEffect.Play();
        if(other.TryGetComponent<IBurnable>(out IBurnable burnable))
        {
            burnable.ApplyBurn(burnDuration, weaponData.BurnDamagePerTick);
        }
        transform.SetParent(other.transform);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        Destroy(gameObject, 1.5f); 
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (hasStuck) return;
        hasStuck = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.SetParent(other.transform);

        if (flameEffect != null && !flameEffect.isPlaying)
            flameEffect.Play();

        float dist = Vector3.Distance(transform.position, other.transform.position);
        if (dist < 2.5f)
        {
            //damage over time
            IBurnable burnable = other.GetComponent<IBurnable>();
            if (burnable != null)
            {
                burnable.ApplyBurn(burnDuration, burnDamage);
                
            }

            //fallback because no enemy has iburnable on it yet
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(Mathf.RoundToInt(burnDamage));
            }
        }
    }
    */
}
