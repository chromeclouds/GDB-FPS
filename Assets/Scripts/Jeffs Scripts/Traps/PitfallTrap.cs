using TMPro;
using UnityEngine;

public class PitfallTrap : MonoBehaviour, ITrapToggle
{
    public GameObject tileToDisable;
    public float delay = 0.5f;
    public LayerMask enemyLayerMask;

    private bool isActive = true;
    private bool hasFallen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || hasFallen) return;

        if ((enemyLayerMask.value & (1 << other.gameObject.layer))==0) return;
        Invoke(nameof(DisableTile), delay);

    }

    private void DisableTile()
    {
        if(tileToDisable != null)
        { 
            tileToDisable.SetActive(false);
            hasFallen = true;
        }
    }

    public void SetTrapActive(bool active)
    {
        isActive = active;
        if(!isActive && tileToDisable != null && hasFallen)
        {
            CancelInvoke();
            tileToDisable.SetActive(true);
            hasFallen = false;
        }
    }
}
