using UnityEngine;

public class BuyableAmmo : MonoBehaviour, ICost
{
    public enum ArmorType { Light, Medium, Heavy };

    [SerializeField] private ArmorType armorType = ArmorType.Light;
    [SerializeField] private int price = 100;
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
                    GiveArmorToPlayer(playerCollider);
                    gameManager.instance.reduceWallet(price);
                }
                else
                {
                    Debug.LogWarning("no collider on player");
                }
            }
        }
    }

    private void GiveArmorToPlayer(Collider playerCollider)
    {
        if (playerCollider == null) return;

        var controller = playerCollider.GetComponent<unifiedPlayerController>();
        if(controller != null)
        {
            switch(armorType)
            {
                case ArmorType.Light:
                    controller.addArmor(1);
                    break;
                case ArmorType.Medium:
                    controller.addArmor(2);
                    break;
                case ArmorType.Heavy:
                    controller.addArmor(3);
                    break;
            }
        }
    }

    public int checkPrice()
    {
        return price;
    }
}
