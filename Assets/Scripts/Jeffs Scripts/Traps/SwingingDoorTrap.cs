using UnityEngine;

public class SwingingDoorTrap : MonoBehaviour, ITrapToggle
{
    [Header("Animation & DMG")]
    public Animator doorAnimator;
    public Collider blockingCollider; //not trigger
    public Collider damageZone; //trigger
    public int damage = 10;

    [Header("Settings")]
    public LayerMask validLayers;

    private bool isActive = true;
    

    public void SetTrapActive(bool active)
    {
        isActive = active;
        if (!isActive)
        {
            blockingCollider.enabled = false;
            damageZone.enabled = false;
            doorAnimator.ResetTrigger("Swing");
            doorAnimator.Play("IdleOpen", 0,0);
        }
        else
        {
            blockingCollider.enabled=true;
            damageZone.enabled=true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || (validLayers.value & (1 << other.gameObject.layer)) == 0) return;
        doorAnimator.SetTrigger("Swing");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive || !damageZone.enabled) return;
        if (other == damageZone) return;
        if((validLayers.value&(1<<other.gameObject.layer))!= 0)
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if(dmg!=null)
            {
                dmg.takeDamage(damage);
                damageZone.enabled = false;
                Invoke(nameof(EnableDamageZone), 3f);
            }
        }
    }

    private void EnableDamageZone()
    {
        if (isActive)
        {
            damageZone.enabled = true;
        }
    }

    
}
