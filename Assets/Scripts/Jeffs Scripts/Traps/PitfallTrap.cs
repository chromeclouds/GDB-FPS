using UnityEngine;

public class PitfallTrap : BaseTrap
{
    public GameObject tileToDisable;
    public float delay = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive ) return;
        Invoke(nameof(DisableTile), delay);

    }

    private void DisableTile()
    {
        if(tileToDisable != null) { tileToDisable.SetActive(false); }
    }
}
