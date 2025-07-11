using System.ComponentModel;
using UnityEngine;

public class SpikeFloorTrap : MonoBehaviour
{
    public Animator spikeAnimator;
    public int damage = 10;
    public LayerMask enemyLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyLayerMask.value & (1 << other.gameObject.layer)) == 0) return;
        spikeAnimator.SetTrigger("Spike");
        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null)
        {
            dmg.takeDamage(damage);
        }
    }
}
