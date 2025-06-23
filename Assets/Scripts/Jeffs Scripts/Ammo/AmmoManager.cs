using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    private Dictionary<AmmoType, int> ammoCounts = new Dictionary<AmmoType, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ini ammo counts
        foreach (AmmoType type in System.Enum.GetValues(typeof(AmmoType)))
        {
            ammoCounts[type] = 0;
        }
    }

    public int GetAmmoCount(AmmoType type)
    {
        return ammoCounts[type];
    }

    public void AddAmmo(AmmoType type, int count)
    {
        ammoCounts[type] += count;
    }

    public bool ConsumeAmmo(AmmoType type, int amount)
    {
        if (ammoCounts[type] >= amount)
        {
            ammoCounts[type] -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
