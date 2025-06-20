using UnityEngine;

public class pickup : MonoBehaviour
{
    [SerializeField] WeaponStats weapon;
    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupable = other.GetComponent<IPickup>();

        if (pickupable != null)
        {
            pickupable.getWeaponStats(weapon);
            weapon.ammoCur = weapon.ammoMax;
            Destroy(gameObject);
        }
    }
}
