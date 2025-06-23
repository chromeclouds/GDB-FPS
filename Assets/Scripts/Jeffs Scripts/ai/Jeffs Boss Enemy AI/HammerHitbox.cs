using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    public int damage = 1;
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHit && other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null )
            {
                dmg.takeDamage(damage);
                hasHit = true;
            }
        }
    }

    public void ResetHit()
    {
        hasHit = false;
    }
}
