using System.Threading;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform bulletSpawnPoint;

    private float fireTimer;
    private int currentAmmo;

    void Start()
    {
        currentAmmo = weaponData.maxAmmo;

    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetButton("Fire1") && fireTimer >= weaponData.fireRate && currentAmmo > 0)
        {
            Fire();
            fireTimer = 0f;
        }

        if (Input.GetKey(KeyCode.R))
        {
            Reload();
        }
    }

    void Fire()
    {
        if (currentAmmo <= 0) return;
        
        currentAmmo--;
        
        if (weaponData.muzzleFlash != null)
        {
            weaponData.muzzleFlash.Play();
        }


        //fire bullet
        for (int i = 0; i < weaponData.bulletsPerShot; i++)
        {
            Quaternion spread = Quaternion.Euler(
                Random.Range(-weaponData.spreadAngle, weaponData.spreadAngle),
                Random.Range(-weaponData.spreadAngle, weaponData.spreadAngle),
                0
                );

            GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation * spread);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * weaponData.bulletSpeed;
            }
        }

        //recoil
        WeaponRecoil recoilScript = GetComponent<WeaponRecoil>();
        if (recoilScript != null)
        {
            recoilScript.Applyrecoil(weaponData.recoilKickback, weaponData.recoilRecoverySpeed);
        }
    }
    
    void Reload()
    {
        currentAmmo = weaponData.maxAmmo;
    }

}
