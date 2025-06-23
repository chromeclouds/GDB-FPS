using UnityEngine;

public class buyableAmmo : MonoBehaviour, ICost
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amount = 30;
    [SerializeField] private int price = 100;
    public void buy()
    {
        if (gameManager.instance.walletAmount() - price >= 0)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null)
                {
                    GiveAmmoToPlayer(playerCollider);
                    gameManager.instance.reduceWallet(price);
                }
                else
                {
                    Debug.LogWarning("player does not have collider");
                }
            }
        }
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

    public int checkPrice()
    {
        return price;
    }
}
