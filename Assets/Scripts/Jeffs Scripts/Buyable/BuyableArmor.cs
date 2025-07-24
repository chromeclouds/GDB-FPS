using UnityEngine;

public class BuyableAmmo : MonoBehaviour, ICost
{
    public enum ArmorType { Light, Medium, Heavy };

    [SerializeField] private ArmorType armorType = ArmorType.Light;
    [SerializeField] private int price = 100;


    public void buy()
    {
        if (gameManager.instance.walletAmount() < price) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        var controller = player.GetComponent<unifiedPlayerController>();
        if(controller == null)
        {
            Debug.LogWarning("player does not have controller");
            return;
        }
        if (controller.isArmorFull())
        {
            Debug.Log("armor is full");
            return;
        }
        gameManager.instance.reduceWallet(price);
        GiveArmorToPlayer(controller);
    }

    private void GiveArmorToPlayer(unifiedPlayerController controller)
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

    public int checkPrice()
    {
        return price;
    }
}
