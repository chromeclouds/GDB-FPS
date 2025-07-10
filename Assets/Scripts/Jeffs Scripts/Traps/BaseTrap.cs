using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour, ICost
{
    [Header("Trap Settings")]
    public int trapCost = 200;
    protected bool isActive = false;
    protected bool isPurchased = false;

    [Tooltip("Unique ID to identify the trap per scene/map")]
    public string trapID;

    public virtual void buy()
    {
        if(!isPurchased && gameManager.instance.walletAmount() >= trapCost)
        {
            gameManager.instance.reduceWallet(trapCost);
            isPurchased=true;
            ActivateTrap();
        }
    }

    public int checkPrice()
    {
        return trapCost;
    }

    public virtual void ActivateTrap()
    {
        isActive = true;
    }

    public virtual void DeactivateTrap()
    {
        isActive = false;
    }

    protected virtual void Start()
    {
        //can extend to support save load or persist manager if someone wants to do that
        if (isPurchased)
            ActivateTrap();
        else
            DeactivateTrap();
        
    }


}
