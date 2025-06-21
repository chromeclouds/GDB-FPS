using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float stickDuration = 3f;
    public float burnDuration = 2f;
    public float burnDamage = 5f;

    private bool hasStuck = false;

    private void Start()
    {
        Destroy(gameObject, stickDuration + burnDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasStuck) return;
        hasStuck = true;

        //stick to surfaces
        transform.SetParent(other.transform);
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;

        //damage over time
        IBurnable burnable = other.GetComponent<IBurnable>();
        if (burnable != null)
        {
            burnable.ApplyBurn(burnDuration, burnDamage);
        }
    }
}
