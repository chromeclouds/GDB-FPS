using UnityEngine;
using UnityEngineInternal;

public class buyTurret : MonoBehaviour, ICost
{
    
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private int price = 100;

    public void buy()
    {
        if (gameManager.instance.walletAmount() >= price)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var controller = player.GetComponent<unifiedPlayerController>();

                if (player != null)
                {

                    gameManager.instance.reduceWallet(price);
                    gameObject.SetActive(false);
                    turretPrefab.SetActive(true);
                }

            }

        }
    }

    public int checkPrice()
    {
        return price;
    }
}
