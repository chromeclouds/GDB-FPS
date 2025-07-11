using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    public int damage = 1;
    private bool canDamage = false;
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage || hasHit) return;
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(damage);
                hasHit = true;
            }
        }
    }

    public void EnableDamage()
    {
        canDamage = true;
        hasHit = false;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }
    
}
