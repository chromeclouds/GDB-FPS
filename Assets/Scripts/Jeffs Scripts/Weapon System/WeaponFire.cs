using System.Collections;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    private AudioSource audioSource;

    public WeaponData weaponData;
    public Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem muzzleFlash;

    private float fireTimer;
    
    private int currentAmmo;
    public int CurrentAmmo => currentAmmo;

    private bool isFiringBurst;
    private int shotsFiredInBurst;
    private bool isReloading = false;
    private bool isOverheated = false;
    private float flamethrowerTimer = 0f;
    private AmmoManager ammoManager;

    [HideInInspector] public GameObject weaponWorldPrefab;
    [HideInInspector] public GameObject weaponHeldPrefab;

    private void Awake()
    {
        //audioSource.loop = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; 
        }
    }

    void OnEnable()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("WeaponData is not set on: " + gameObject.name);
            return;
        }

        ammoManager = GetComponentInParent<AmmoManager>();
        currentAmmo = weaponData.MaxAmmo;

    }

    void Update()
    {
        if (isReloading || isOverheated) return;
        if (GetComponentInParent<unifiedPlayerController>() == null) return;

        fireTimer += Time.deltaTime;

        if (weaponData.IsFlamethrower)
        {
            if (Input.GetButton("Fire1"))
            {
                Fire();
                flamethrowerTimer += Time.deltaTime;

                if (!audioSource.isPlaying && weaponData.FireSound != null)
                {
                    audioSource.clip = weaponData.FireSound;
                    audioSource.loop = false;
                    audioSource.Play();
                }

                if (flamethrowerTimer >= weaponData.OverheatTime)
                {
                    StartCoroutine(Overheat());
                }
            }
            else
            {
                flamethrowerTimer = Mathf.Max(0f, flamethrowerTimer - Time.deltaTime);

                if (audioSource.isPlaying && audioSource.clip == weaponData.FireSound)
                    audioSource.Stop();

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

            if (Input.GetKeyDown(KeyCode.R) && !weaponData.HasInfiniteAmmo && ammoManager.GetAmmoCount(weaponData.AmmotType) > 0 && currentAmmo < weaponData.MaxAmmo)
            {
                StartCoroutine(Reload());
            }
        }
    }
    private void PlayMuzzleFlash()
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }
    void Fire()
    {
        if (!weaponData.HasInfiniteAmmo && currentAmmo <= 0)
        {
            PlaySound(weaponData.EmptyClickSound);
            return;
        }

        if (!weaponData.HasInfiniteAmmo)
        {
            currentAmmo--;
            if(weaponData.FireSound != null && !weaponData.IsFlamethrower)
                PlaySound(weaponData.FireSound);
        }
            

        WeaponUIManager.instance.UpdateAmmoCount(CurrentAmmo, ammoManager.GetAmmoCount(weaponData.AmmotType));

        /*if (weaponData.MuzzleFlash != null)
            weaponData.MuzzleFlash.Play();
        if (weaponData.FireSound != null)
            AudioSource.PlayClipAtPoint(weaponData.FireSound, transform.position);
        */

        for (int i = 0; i < weaponData.BulletsPerShot; i++)
        {
            Vector3 targetPoint = Camera.main.transform.position + Camera.main.transform.forward * 100f;
            Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;
            Quaternion spreadRotation = Quaternion.Euler(
                Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle),
                Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle), 0);

            GameObject bullet = Instantiate(weaponData.BulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction) * spreadRotation);
            FireBullet fire = bullet.GetComponent<FireBullet>();
            PlayMuzzleFlash();
            if (fire != null)
            {
                fire.weaponData = weaponData;
            }
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = weaponData.Damage;
            }

            ExplosiveBullet explosive = bullet.GetComponent<ExplosiveBullet>();
            if (explosive != null)
            {
                explosive.damage = weaponData.Damage;
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * weaponData.BulletSpeed;
            }
        }

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

        if(weaponData.ReloadStartSound != null)
            PlaySound(weaponData.ReloadStartSound);

        yield return new WaitForSeconds(weaponData.ReloadTime);

        int ammoNeeded = weaponData.MaxAmmo - currentAmmo;
        int availableAmmo = ammoManager.GetAmmoCount(weaponData.AmmotType);
        int ammoToLoad = Mathf.Min(ammoNeeded, availableAmmo);

        if (ammoToLoad > 0)
        {
            ammoManager.ConsumeAmmo(weaponData.AmmotType, ammoToLoad);
            currentAmmo += ammoToLoad;
            WeaponUIManager.instance.UpdateAmmoCount(CurrentAmmo, ammoManager.GetAmmoCount(weaponData.AmmotType));

        }

        if (weaponData.ReloadEndSound != null)
            PlaySound(weaponData.ReloadEndSound);

        isReloading = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
