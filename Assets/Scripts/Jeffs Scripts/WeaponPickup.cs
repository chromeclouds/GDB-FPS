using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    public WeaponData weaponData;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.PickupWeapon(weaponPrefab, weaponData); 
                Destroy(gameObject); //destroy world prefab
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
