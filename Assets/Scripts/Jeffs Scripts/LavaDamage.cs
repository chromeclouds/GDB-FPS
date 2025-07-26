using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaDamage : MonoBehaviour
{
    [Tooltip("Damage to apply per tick")]
    public int damageAmount = 10;

    [Tooltip("Time between damage ticks (in seconds)")]
    public float damageInterval = 1f;

    private HashSet<IDamage> playersInLava = new HashSet<IDamage>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && !playersInLava.Contains(dmg))
        {
            playersInLava.Add(dmg);
            StartCoroutine(DamageOverTime(dmg));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && playersInLava.Contains(dmg))
        {
            playersInLava.Remove(dmg);
        }
    }

    private IEnumerator DamageOverTime(IDamage dmg)
    {
        while (playersInLava.Contains(dmg))
        {
            dmg.takeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
