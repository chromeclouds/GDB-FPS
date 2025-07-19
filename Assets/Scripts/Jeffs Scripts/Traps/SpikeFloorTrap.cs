using System.ComponentModel;
using UnityEngine;

public class SpikeFloorTrap : MonoBehaviour, ITrapToggle
{
    public Animator spikeAnimator;
    public int damage = 10;
    public LayerMask enemyLayerMask;
    private bool isActive = true;


    public void SetTrapActive(bool active)
    {
        isActive = active;
        if (!isActive && spikeAnimator != null)
        {
            spikeAnimator.ResetTrigger("Spike");
            spikeAnimator.Play("Idle", 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || (enemyLayerMask.value & (1 << other.gameObject.layer)) == 0) return;
        spikeAnimator.SetTrigger("Spike");
        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null)
        {
            dmg.takeDamage(damage);
        }
    }
}
