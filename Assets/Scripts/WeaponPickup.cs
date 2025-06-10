using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponSwap weaponSwap = other.GetComponent<PlayerWeaponSwap>();
            if (weaponSwap != null)
            {
                weaponSwap.PickupWeapon(weaponPrefab); //give held prefab
                Destroy(transform.parent.gameObject); //destroy world prefab
            }
        }
    }
        


    void Start()
    {
        
    }



    void Update()
    {
        
    }
}
