using UnityEngine;

public class PitfallTrap : MonoBehaviour
{
    public GameObject tileToDisable;
    public float delay = 0.5f;
    public LayerMask enemyLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyLayerMask.value & (1 << other.gameObject.layer))==0) return;
        Invoke(nameof(DisableTile), delay);

    }

    private void DisableTile()
    {
        if(tileToDisable != null) { tileToDisable.SetActive(false); }
    }
}
