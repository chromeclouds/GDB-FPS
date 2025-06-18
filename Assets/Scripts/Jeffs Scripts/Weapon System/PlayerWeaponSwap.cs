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

        currentWeapon.transform.localPosition = weaponData.WeaponPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(weaponData.WeaponRotationOffset);

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
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeaponToNearbyCrate();
        }
    }

    void DropWeaponToNearbyCrate()
    {
        if (currentWeapon == null) return;

        WeaponData weaponData = currentWeapon.GetComponent<WeaponFire>()?.weaponData;
        if (weaponData == null) return;

        WeaponCrate nearestCrate = FindClosestCrate();

        if (nearestCrate != null && nearestCrate.GetCurrentItem() == null)
        {
            nearestCrate.PlaceItem(currentWeapon.GetComponent<WeaponPickup>().weaponPrefab);
            Destroy(currentWeapon);
            currentWeapon = null;
        }
    }

    WeaponCrate FindClosestCrate()
    {
        WeaponCrate[] crates = Object.FindObjectsByType<WeaponCrate>(FindObjectsSortMode.None);
        WeaponCrate nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach ( var crate in crates )
        {
            float dist = Vector3.Distance(pos, crate.transform.position);
            if (dist < minDist && dist <= 3f)
            {
                minDist = dist;
                nearest = crate;
            }
        }
        return nearest;
    }
}
