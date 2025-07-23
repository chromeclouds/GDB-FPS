using Unity.VisualScripting;
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

                //crate has item
                if (crateItem != null)
                {
                    //if 3 weapons swap held
                    if (player.HasMaxWeapons())
                    {
                        GameObject toDrop = player.RemoveCurrentHeldWeapon();
                        if (toDrop != null)
                        {
                            WeaponFire fire = toDrop.GetComponent<WeaponFire>();
                            if (fire != null && fire.weaponWorldPrefab != null)
                            {
                                GameObject world = Instantiate(fire.weaponWorldPrefab);
                                PositionAndAttachToCrate(world, closestCrate);
                                closestCrate.PlaceItem(world);
                            }
                            Destroy(toDrop);
                        }
                    }
                    else
                    {
                        //clear the crate item
                        closestCrate.ClearItem();
                    }

                    PickupWeapon(crateItem);
                }

                //crate is empty and player is holding a weapon
                else if (heldWeapon != null)
                {
                    GameObject toDrop = player.RemoveCurrentHeldWeapon(true, closestCrate.itemHolder);
                    if(toDrop != null)
                    {
                        closestCrate.PlaceItem(toDrop);
                    }
                }
            }
        

    }

    void PlaceWeapon(GameObject heldWeapon, WeaponCrate crate)
    {
        if (heldWeapon == null || crate == null || player == null) return;

        //get weaponfire &data
        WeaponFire fire = heldWeapon.GetComponent<WeaponFire>(); 
        if (fire == null || fire.weaponWorldPrefab == null) return;

        player.RemoveCurrentHeldWeapon();

        GameObject toPlace = Instantiate(fire.weaponWorldPrefab);
        if(toPlace!= null)
        {
            var fireScript = toPlace.GetComponent<WeaponFire>();
            if (fireScript != null)
                fireScript.enabled = false;

            foreach (Collider col in toPlace.GetComponentsInChildren<Collider>())
                col.enabled = true;

            var pickup = toPlace.GetComponent<unifiedWeaponPickup>();
            if (pickup != null)
                pickup.enabled = true;

            PositionAndAttachToCrate(toPlace, crate);
            crate.PlaceItem(toPlace);
            /*
            toPlace.transform.SetParent(null);
            toPlace.transform.position = crate.itemHolder.position;
            toPlace.transform.rotation = Quaternion.identity;

            
            
            

            CrateItem crateItem = toPlace.GetComponent<CrateItem>();
            if (crateItem == null)
                crateItem = toPlace.AddComponent<CrateItem>();
            crateItem.originCrate = crate;
            */
            
        }
        
    }


    void PickupWeapon(GameObject crateItem)
    {
        if (crateItem == null || player == null) return;

        var pickup = crateItem.GetComponent<unifiedWeaponPickup>();
        if (pickup == null || pickup.weaponData == null || pickup.weaponPrefab == null) return;

        WeaponData data = pickup.weaponData;
        GameObject heldPrefab = pickup.weaponPrefab;

        player.getWeaponData(data, heldPrefab);
        Destroy(crateItem);

        /*
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
        */
    }

    void PositionAndAttachToCrate(GameObject obj, WeaponCrate crate)
    {
        obj.transform.position = crate.itemHolder.position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.SetParent(null);

        CrateItem crateItem = obj.GetComponent<CrateItem>();
        if(crateItem == null)
            crateItem = obj.AddComponent<CrateItem>();
        crateItem.originCrate = crate;
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