using UnityEngine;

public class SwingingDoorTrap : MonoBehaviour, ICost
{
    public Animator doorAnimator;
    public int trapCost = 200;
    private bool isPurchased = false;
    public LayerMask enemyLayerMask;

    public void buy()
    {
        if (isPurchased) return;
        if(gameManager.instance.walletAmount() >= trapCost)
        {
            gameManager.instance.reduceWallet(trapCost);
            isPurchased = true;
        }
    }

    public int checkPrice()
    {
        return trapCost;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPurchased || (enemyLayerMask.value & (1 << other.gameObject.layer)) == 0) return;
        doorAnimator.SetTrigger("Swing");
    }
}
