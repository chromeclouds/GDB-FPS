using UnityEngine;
using UnityEngineInternal;

public class BuyableWeapon : MonoBehaviour, ICost
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private int price = 500;

    public void buy()
    {
        if (gameManager.instance.walletAmount() - price >= 0)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null)
                {
                    GiveWeaponToPlayer(playerCollider);
                    gameManager.instance.reduceWallet(price);
                    gameObject.SetActive(false); //remove if you want it to stay on the wall.
                }
                else
                {
                    Debug.LogWarning("Player has no collider");
                }
            }

        }
    }
    
    private void GiveWeaponToPlayer(Collider playerCollider)
    {
        if (playerCollider == null) return;

        unifiedPlayerController controller = playerCollider.GetComponent<unifiedPlayerController>();
        if(controller != null)
        {
            controller.getWeaponData(weaponData, weaponPrefab);
        }
    }

    public int checkPrice()
    {
        return price;
    }
}
