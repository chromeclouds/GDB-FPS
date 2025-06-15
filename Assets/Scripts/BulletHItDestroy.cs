using JetBrains.Annotations;
using UnityEngine;

public class BulletHitDestroy : MonoBehaviour
{
    [SerializeField] int damageAmount = 10; // Default damage per bullet
    [SerializeField] int damageRate = 1;    // Multiplier for rapid-fire weapons

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            int totalDamage = damageAmount * damageRate; // Scaled damage
            dmg.takeDamage(totalDamage);
        }

        Destroy(gameObject);
    }
}
