using UnityEngine;

public class unifiedCrateInteractor : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.G;
    private unifiedPlayerController player;

    void Start()
    {
        player = GetComponent<unifiedPlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            WeaponCrate closestCrate = FindNearestCrate();
            if (closestCrate == null) return;

            GameObject heldWeapon = player.GetCurrentHeldWeapon();
            GameObject crateItem = closestCrate.GetCurrentItem();

            // Place weapon if player has one and crate is empty
            if (heldWeapon != null && crateItem == null)
            {
                PlaceWeapon(heldWeapon, closestCrate);
            }
            // Pick up weapon if crate has item and player has room
            else if (heldWeapon == null && crateItem != null)
            {
                PickupWeapon(crateItem, closestCrate);
            }
        }
    }

    void PlaceWeapon(GameObject heldWeapon, WeaponCrate crate)
    {
        if (heldWeapon == null || crate == null || player == null) return;

        //get weaponfire &data
        WeaponFire fire = heldWeapon.GetComponent<WeaponFire>();
        if (fire == null || fire.weaponWorldPrefab == null) return;

        GameObject toPlace = player.RemoveCurrentHeldWeapon();
        if(toPlace!= null)
        {
            var fireScript = toPlace.GetComponent<WeaponFire>();
            if (fireScript != null)
                fireScript.enabled = false;

            toPlace.transform.SetParent(null);
            toPlace.transform.position = crate.itemHolder.position;
            toPlace.transform.rotation = Quaternion.identity;

            foreach (Collider col in toPlace.GetComponentsInChildren<Collider>())
                col.enabled = true;
            
            var pickup = toPlace.GetComponent<unifiedWeaponPickup>();
            if (pickup != null)
                pickup.enabled = true;

            CrateItem crateItem = toPlace.GetComponent<CrateItem>();
            if (crateItem == null)
                crateItem = toPlace.AddComponent<CrateItem>();
            crateItem.originCrate = crate;
            
            crate.PlaceItem(toPlace);
        }
        
    }


    void PickupWeapon(GameObject crateItem, WeaponCrate crate)
    {
        if (crate == null || crateItem == null || player == null) return;

        var pickup = crateItem.GetComponent<unifiedWeaponPickup>();
        if (pickup == null || pickup.weaponData == null || pickup.weaponPrefab == null) return;

        WeaponData data = pickup.weaponData;
        GameObject heldPrefab = pickup.weaponPrefab;

        if (player.HasMaxWeapons())
        {
            GameObject toDrop = player.RemoveCurrentHeldWeapon();
            if (toDrop != null)
            {
                WeaponFire fire = toDrop.GetComponent<WeaponFire>();
                if(fire!= null && fire.weaponWorldPrefab != null)
                {
                    GameObject world = Instantiate(fire.weaponWorldPrefab);
                    world.transform.position = crate.itemHolder.position;
                    crate.PlaceItem(world);

                }
                Destroy(toDrop);
            }
        }
        else
        {
            crate.ClearItem();
        }
        player.getWeaponData(data, heldPrefab);
    }





    WeaponCrate FindNearestCrate()
    {
        WeaponCrate[] crates = Object.FindObjectsByType<WeaponCrate>(FindObjectsSortMode.None);
        WeaponCrate closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var crate in crates)
        {
            float dist = Vector3.Distance(transform.position, crate.transform.position);
            if (dist < interactRange && dist < closestDist)
            {
                closest = crate;
                closestDist = dist;
            }
        }
        return closest;
    }
}