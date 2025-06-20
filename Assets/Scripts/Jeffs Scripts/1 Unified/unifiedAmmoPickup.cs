using UnityEngine;

public class unifiedAmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amount = 30;
    private CrateItem crateOrigin;

    void Start()
    {
        crateOrigin = GetComponent<CrateItem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AmmoManager ammoManager = other.GetComponent<AmmoManager>();
        if (ammoManager != null)
        {
            ammoManager.AddAmmo(ammoType, amount);
            ClearFromCrate();
            Destroy(gameObject);
        }
    }

    void ClearFromCrate()
    {
        if (crateOrigin != null && crateOrigin.originCrate != null)
        {
            crateOrigin.originCrate.ClearItem();
        }
    }
}
