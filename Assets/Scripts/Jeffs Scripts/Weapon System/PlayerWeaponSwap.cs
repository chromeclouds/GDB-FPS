using UnityEngine;

public class PlayerWeaponSwap : MonoBehaviour
{
    public GameObject currentWeapon;
    public Transform weaponHolder; //empty gameobject as attachment point "hands"
    public GameObject currentWorldPrefab;

    public void PickupWeapon(GameObject weaponPrefab, WeaponData weaponData)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWorldPrefab = weaponPrefab;

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
        Debug.Log("drop weapon to nearby crate triggered");

        if (currentWeapon == null || currentWorldPrefab == null)
        {
            Debug.LogWarning("no weapon to drop or missing prefab ref");
            return;
        }

        WeaponCrate nearestCrate = FindClosestCrate();
        Debug.Log("nearest crate: " + (nearestCrate != null ? nearestCrate.name : "none"));

        if (nearestCrate != null && nearestCrate.GetCurrentItem() == null)
        {
            Debug.Log("placing item on crate");
            nearestCrate.PlaceItem(currentWorldPrefab);
            Destroy(currentWeapon);
            currentWeapon = null;
            currentWorldPrefab = null;
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
            if (dist < minDist && dist <= 5f)
            {
                minDist = dist;
                nearest = crate;
            }
        }
        return nearest;
    }
}
