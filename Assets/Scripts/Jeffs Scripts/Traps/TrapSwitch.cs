using Unity.VisualScripting;
using UnityEngine;

public class TrapSwitch : MonoBehaviour, ICost
{
    [Header("Switch Settings")]
    public Transform switchHandle;
    public Vector3 onRotation = new Vector3(-45, 0, 0);
    public Vector3 offRotation = new Vector3(45, 0, 0);

    public bool startsOn = true;
    public int trapCost = 100;
    private bool isPurchased = false;
    private bool isActive;

    [Header("Connected Trap")]
    public MonoBehaviour connectedTrap; //should implement itraptoggle

    void Start()
    {
        isActive = startsOn;
        UpdateVisual();
        ToggleTrap(isActive);
    }
    

    public void buy()
    {
        if (!isPurchased)
        {
            if (gameManager.instance.walletAmount() >= trapCost)
            {
                gameManager.instance.reduceWallet(trapCost);
                isPurchased = true;
                Toggle();
            }
            else
            {
                Debug.Log("not enough to buy");
                
            }
        }
        else
        {
            Toggle();
        }
    }

    public int checkPrice()
    {
        return isPurchased ? 0 : trapCost;

    }

    private void Toggle()
    {
        isActive = !isActive;
        UpdateVisual();
        ToggleTrap(isActive);
    }

    void UpdateVisual()
    {
        if (switchHandle != null)
        {
            switchHandle.localEulerAngles = isActive ? onRotation : offRotation;

        }
    }

    void ToggleTrap(bool enable)
    {
        if (connectedTrap != null && connectedTrap is ITrapToggle toggle)
        {
            toggle.SetTrapActive(enable);
        }
    }
}

public interface ITrapToggle
{
    void SetTrapActive(bool active);
}
