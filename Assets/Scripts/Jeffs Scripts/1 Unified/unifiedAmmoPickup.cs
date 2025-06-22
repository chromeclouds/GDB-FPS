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

            //update ui on pickup
            var controller = other.GetComponent<unifiedPlayerController>();
            if (controller != null) 
            {
                GameObject currentWeapon = controller.GetCurrentHeldWeapon();
                if (currentWeapon != null)
                {
                    WeaponFire fire = currentWeapon.GetComponent<WeaponFire>();
                    if (fire != null && fire.weaponData.AmmotType == ammoType)
                    {
                        WeaponUIManager.instance.UpdateAmmoCount(fire.CurrentAmmo, ammoManager.GetAmmoCount(ammoType));
                    }
                }
            }
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
