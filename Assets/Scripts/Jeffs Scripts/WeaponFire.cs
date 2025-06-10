using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform bulletSpawnPoint;

    private float fireTimer;
    private int currentAmmo;
    private bool isFiringBurst;
    private int shotsFiredInBurst;

    void Start()
    {
        currentAmmo = weaponData.maxAmmo;

    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        switch (weaponData.fireMode)
        {
            case FireMode.FullAuto:
                if (Input.GetButton("Fire1") && fireTimer >= weaponData.fireRate && currentAmmo > 0)
                {
                    Fire();
                    fireTimer = 0f;
                }
                break;
            case FireMode.SemiAuto:
                if (Input.GetButtonDown("Fire1") && fireTimer >= weaponData.fireRate && currentAmmo > 0)
                {
                    Fire();
                    fireTimer = 0f;
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.R))
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

   private IEnumerator BurstFire()
    {
        isFiringBurst = true;
        shotsFiredInBurst = 0;

        while (shotsFiredInBurst < weaponData.burstCount && currentAmmo > 0)
        {
            Fire();
            shotsFiredInBurst++;
            fireTimer = 0f;
            yield return new WaitForSeconds(weaponData.fireRate);
        }

        isFiringBurst = false;
    }
    
    void Reload()
    {
        currentAmmo = weaponData.maxAmmo;
    }

}
