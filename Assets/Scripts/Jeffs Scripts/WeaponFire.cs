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
    private bool isReloading = false;
    private bool isOverheated = false;
    private float flamethrowerTimer = 0f;
    private AmmoManager ammoManager;

    void Start()
    {
        ammoManager = GetComponentInParent<AmmoManager>();
        currentAmmo = weaponData.MaxAmmo;

    }

    void Update()
    {
        if (isReloading || isOverheated) return;

        fireTimer += Time.deltaTime;

        if (weaponData.IsFlamethrower)
        {
            if (Input.GetButton("Fire1"))
            {
                Fire();
                flamethrowerTimer += Time.deltaTime;

                if (flamethrowerTimer >= weaponData.OverheatTime)
                {
                    StartCoroutine(Overheat());
                }
            }
            else
            {
                //if player lets go of fire, cool down
                flamethrowerTimer = Mathf.Max(0f, flamethrowerTimer - Time.deltaTime);
            }
        }
        else
        {
            switch (weaponData.FireMode)
            {
                case FireMode.FullAuto:
                    if (Input.GetButton("Fire1") && fireTimer >= weaponData.FireRate && currentAmmo > 0)
                    {
                        Fire();
                        fireTimer = 0f;
                    }
                    break;
                case FireMode.SemiAuto:
                    if (Input.GetButtonDown("Fire1") && fireTimer >= weaponData.FireRate && currentAmmo > 0)
                    {
                        Fire();
                        fireTimer = 0f;
                    }
                    break;
                case FireMode.Burst:
                    if (Input.GetButton("Fire1") && !isFiringBurst && currentAmmo > 0)
                    {
                        StartCoroutine(BurstFire());
                    }
                    break;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Fire()
    {
        if (currentAmmo <= 0) return;
        
        currentAmmo--;
        
        if (weaponData.MuzzleFlash != null)
        {
            weaponData.MuzzleFlash.Play();
        }


        //fire bullet
        for (int i = 0; i < weaponData.BulletsPerShot; i++)
        {
            Quaternion spread = Quaternion.Euler(
                Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle),
                Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle),
                0
                );

            GameObject bullet = Instantiate(weaponData.BulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation * spread);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = weaponData.Damage; //pass in weapon damage. 
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * weaponData.BulletSpeed;
            }
        }

        //recoil
        WeaponRecoil recoilScript = GetComponent<WeaponRecoil>();
        if (recoilScript != null)
        {
            recoilScript.Applyrecoil(weaponData.RecoilKickback, weaponData.RecoilRecoverySpeed);
        }
    }

    private IEnumerator Overheat()
    {
        isOverheated = true;
        Debug.Log("Overheated!");
        yield return new WaitForSeconds(weaponData.CooldownTime);
        flamethrowerTimer = 0f;
        isOverheated = false;
    }

   private IEnumerator BurstFire()
    {
        isFiringBurst = true;
        shotsFiredInBurst = 0;

        while (shotsFiredInBurst < weaponData.BurstCount && currentAmmo > 0)
        {
            Fire();
            shotsFiredInBurst++;
            fireTimer = 0f;
            yield return new WaitForSeconds(weaponData.FireRate);
        }

        isFiringBurst = false;
    }
    
    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading..");
        yield return new WaitForSeconds(weaponData.ReloadTime);

        int ammoNeeded = weaponData.MaxAmmo - currentAmmo;
        int availableAmmo = ammoManager.GetAmmoCount(weaponData.AmmotType);
        int ammoToLoad = Mathf.Min(ammoNeeded, availableAmmo);

        if (ammoToLoad > 0)
        {
            ammoManager.ConsumeAmmo(weaponData.AmmotType, ammoToLoad);
            currentAmmo += ammoToLoad;
        }

        isReloading = false;
    }

}
