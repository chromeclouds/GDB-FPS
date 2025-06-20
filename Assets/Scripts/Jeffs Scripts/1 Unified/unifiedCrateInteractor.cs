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
        GameObject dropped = player.RemoveCurrentHeldWeapon();
        if (dropped == null) return;

        dropped.transform.SetParent(null);
        dropped.transform.position = crate.itemHolder.position;
        dropped.transform.rotation = Quaternion.identity;

        var pickup = dropped.GetComponent<unifiedWeaponPickup>();
        if (pickup != null)
        {
            pickup.enabled = true;
        }

        foreach (Collider col in dropped.GetComponentsInChildren<Collider>())
            col.enabled = true;

        crate.PlaceItem(dropped);
    }

    void PickupWeapon(GameObject crateItem, WeaponCrate crate)
    {
        // Remove reference from crate BEFORE modifying the object
        crate.ClearItemWithoutDestroy(); // You’ll define this method below

        crateItem.transform.SetParent(player.weaponHolder);
        crateItem.transform.localPosition = Vector3.zero;
        crateItem.transform.localRotation = Quaternion.identity;

        var pickup = crateItem.GetComponent<unifiedWeaponPickup>();
        if (pickup != null)
            pickup.enabled = false;

        foreach (Collider col in crateItem.GetComponentsInChildren<Collider>())
            col.enabled = false;

        player.AddExistingWeapon(crateItem);
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