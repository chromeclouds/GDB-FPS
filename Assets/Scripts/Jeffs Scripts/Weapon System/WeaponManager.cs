using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{

    public Transform weaponHolder;
    private List<GameObject> ownedWeapons = new List<GameObject>();
    private int currentWeaponIndex = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }

        //can add more to number keys if needed
        
    }

    public void PickupWeapon(GameObject weaponPrefab, WeaponData weaponData)
    {
        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder);
        newWeapon.transform.localPosition = weaponData.WeaponPositionOffset;
        newWeapon.transform.localRotation = Quaternion.Euler(weaponData.WeaponRotationOffset);

        WeaponFire weaponFire = newWeapon.GetComponent<WeaponFire>();
        if(weaponFire != null )
        {
            weaponFire.weaponData = weaponData;
        }
        ownedWeapons.Add(newWeapon);

        //deactivate all but current
        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            ownedWeapons[i].SetActive(i == currentWeaponIndex);
        }
    }
    
    public void ClearWeapons()
    {
        foreach (var weapon in ownedWeapons)
        {
            if (weapon != null) Destroy(weapon);
        }
        ownedWeapons.Clear();
    }

    private void SwitchWeapon(int index)
    {
        if (index >= 0 && index < ownedWeapons.Count)
        {
            ownedWeapons[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = index;
            ownedWeapons[currentWeaponIndex].SetActive(true);
        }
    }
}
