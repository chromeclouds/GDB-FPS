using System.Collections;
using UnityEngine;

public class unifiedAmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amount = 30;
    [SerializeField] private int price = 100;

    private CrateItem crateOrigin;

    void Start()
    {
        crateOrigin = GetComponent<CrateItem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //if buyable and not bought, ignore pickup
        if (GetComponent<ICost>() != null) return;

        //if bought and persist, ignore trigger pickup
        if (gameObject.CompareTag("Bought")) return;

        //otherwise its free
        GiveAmmoToPlayer(other);
        ClearFromCrate();
        Destroy(gameObject);
    }

    //call from staticobjectlogig when bought
    public void Buy()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if(playerCollider != null)
            {
                GiveAmmoToPlayer(playerCollider);
            }
            else
            {
                Debug.LogWarning("player does not have collider");
            }
        }
        StartCoroutine(ResetTag());
    }
    IEnumerator ResetTag()
    {
        yield return new WaitForSeconds(1f); //wait 
        gameObject.tag = "Untagged";
    }

    private void GiveAmmoToPlayer(Collider player)
    {
        if (player == null) return;

        AmmoManager ammoManager = player.GetComponent<AmmoManager>();
        if (ammoManager != null) 
        {
            ammoManager.AddAmmo(ammoType, amount);
            //update ui if holding same ammo type
            var controller = player.GetComponent<unifiedPlayerController>();
            if (controller != null)
            {
                GameObject currentWeapon = controller.GetCurrentHeldWeapon();
                if(currentWeapon != null)
                {
                    WeaponFire fire = currentWeapon.GetComponent<WeaponFire>();
                    if (fire != null && fire.weaponData.AmmotType == ammoType)
                    {
                        WeaponUIManager.instance.UpdateAmmoCount(fire.CurrentAmmo, ammoManager.GetAmmoCount(ammoType));

                    }
                }
            }
        }
    }

    void ClearFromCrate()
    {
        if (crateOrigin != null && crateOrigin.originCrate != null)
        {
            crateOrigin.originCrate.ClearItem();
        }
    }
    public int checkPrice() => price;
    public void buy()
    {
        if (gameManager.instance.walletAmount() >= price)
        {
            gameManager.instance.reduceWallet(price);

            //find player and give ammo 
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                AmmoManager ammoManager = player.GetComponent<AmmoManager>();
                if (ammoManager != null)
                {
                    ammoManager.AddAmmo(ammoType, amount);

                    var controller = player.GetComponent<unifiedPlayerController>();
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
                }
            }

            gameObject.tag = "Bought"; 
            StartCoroutine(ResetTag()); // Allow future purchases
        }
    }
}

