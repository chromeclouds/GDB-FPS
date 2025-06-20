using UnityEngine;

public class WeaponPickup : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            PlayerWeaponSwap weaponSwap = other.GetComponent<PlayerWeaponSwap>();
            if (weaponSwap != null )
            {
                weaponSwap.currentWorldPrefab = weaponPrefab;
                weaponSwap.PickupWeapon(weaponPrefab, weaponData);
            }
            else
            {
                WeaponManager weaponManager = other.GetComponent<WeaponManager>();
                if (weaponManager != null)
                {
                    weaponManager.PickupWeapon(weaponPrefab, weaponData);
                }
            }

            if (crateOrigin != null && crateOrigin.originCrate != null)
            {
                crateOrigin.originCrate.ClearItem();
            }

            Destroy(gameObject);
            
        
        }
    }
}
