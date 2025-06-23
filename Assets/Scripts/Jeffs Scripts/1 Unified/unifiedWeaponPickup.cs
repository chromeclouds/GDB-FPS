using UnityEngine;

public class unifiedWeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    public WeaponData weaponData;
    private CrateItem crateOrigin;

    void Start()
    {
        crateOrigin = GetComponent<CrateItem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Priority 1: my weapon system
        var unifiedController = other.GetComponent<unifiedPlayerController>();
        if (unifiedController != null)
        {
            unifiedController.getWeaponData(weaponData, weaponPrefab);
            ClearFromCrate();
            gameObject.SetActive(false);
            return;
        }

        // Priority 2: fallback to lecture stuff
        IPickup pickupable = other.GetComponent<IPickup>();
        if (pickupable != null)
        {
            pickupable.getWeaponStats(null); // Not used, placeholder for interface
            ClearFromCrate();
            Destroy(gameObject);
            return;
        }

        // Fallback: weapon manager based pick up (my original stuff)
        var weaponManager = other.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.PickupWeapon(weaponPrefab, weaponData);
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
