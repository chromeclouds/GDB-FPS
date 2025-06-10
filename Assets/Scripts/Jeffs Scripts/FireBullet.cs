using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float burnDuration = 5f;
    public int burnDamagePerSecond = 5;
    public GameObject fireEffectPrefab;
    public GameObject burnDecalPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3f); //flame lifespan    
    }

    private void OnTriggerEnter(Collider other)
    {
        IBurnable burnable = other.GetComponent<IBurnable>();

        if (burnable != null)
        {
            burnable.ApplyBurn(burnDuration, burnDamagePerSecond);

            if(fireEffectPrefab)
            {
                Instantiate(fireEffectPrefab, other.transform.position, Quaternion.identity, other.transform);
            }
        }

        //add burn decal to surface
        if (burnDecalPrefab != null)
        {
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            Instantiate(burnDecalPrefab, contactPoint, Quaternion.identity);
        }

        Destroy(gameObject); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
