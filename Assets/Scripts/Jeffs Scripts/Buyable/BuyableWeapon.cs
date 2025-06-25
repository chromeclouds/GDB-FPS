using UnityEngine;
using UnityEngineInternal;

public class BuyableWeapon : MonoBehaviour, ICost
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject weaponHeldPrefab;
    [SerializeField] private int price = 500;

    public void buy()
    {
        if (gameManager.instance.walletAmount() >= price)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                var controller = player.GetComponent<unifiedPlayerController>();
                
                if (player != null)
                {
                    controller.getWeaponData(weaponData, weaponHeldPrefab);
                    
                    gameManager.instance.reduceWallet(price);
                    gameObject.SetActive(false); //remove if you want it to stay on the wall.
                }
                
            }

        }
    }
    
    

    public int checkPrice()
    {
        return price;
    }
}
