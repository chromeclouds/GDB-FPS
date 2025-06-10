using UnityEngine;

public class PlayerWeaponSwap : MonoBehaviour
{
    public GameObject currentWeapon;
    public Transform weaponHolder; //empty gameobject as attachment point "hands"

    public void PickupWeapon(GameObject weaponPrefab, WeaponData weaponData)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weaponPrefab, weaponHolder);

        currentWeapon.transform.localPosition = weaponData.weaponPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(weaponData.weaponRotationOffset);

        WeaponFire weaponFire = currentWeapon.GetComponent<WeaponFire>();
        if(weaponFire != null)
        {
            weaponFire.weaponData = weaponData; //assign weapon data dynamically
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
