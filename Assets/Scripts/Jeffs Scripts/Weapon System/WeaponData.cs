using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{

    [Header("Basic Info")]
    [SerializeField] private string weaponName;
    public string WeaponName => weaponName;

    [Header("Firing")]
    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float spreadAngle;
    [SerializeField] private int bulletsPerShot = 1; //shotgun effect
    [SerializeField] private ParticleSystem muzzleFlash;
    public float FireRate => fireRate;
    public int Damage => damage;
    public float BulletSpeed => bulletSpeed;
    public GameObject BulletPrefab => bulletPrefab;
    public float SpreadAngle => spreadAngle;
    public int BulletsPerShot => bulletsPerShot;
    public ParticleSystem MuzzleFlash => muzzleFlash;


    [Header("Recoil")]
    [SerializeField] private float recoilKickback = 1.0f; //how much kick
    [SerializeField] private float recoilRecoverySpeed = 5.0f; //how fast gun returns to zero
    public float RecoilKickback => recoilKickback;
    public float RecoilRecoverySpeed => recoilRecoverySpeed;


    [Header("ADS")]
    [SerializeField] private bool hasADS = true;
    [SerializeField] private Vector3 adsPositionOffset;
    [SerializeField] private float adsSpeed = 10f;
    [SerializeField] private float adsFOV = 40f;
    [SerializeField] private float adsFOVSpeed = 10f;
    public bool HasADS => hasADS;
    public Vector3 ADSPositionOffset => adsPositionOffset;
    public float ADSSpeed => adsSpeed;
    public float ADSFOV => adsFOV;
    public float ADSFOVSpeed => adsFOVSpeed;


    [Header("Fire Mode")]
    [SerializeField] private FireMode fireMode = FireMode.FullAuto;
    [SerializeField] private int burstCount = 3; //burst guns
    public FireMode FireMode => fireMode;
    public int BurstCount => burstCount;


    [Header("Ammo")]
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private int maxAmmo = 30;
    public AmmoType AmmotType => ammoType;
    public float ReloadTime => reloadTime;
    public int MaxAmmo => maxAmmo;


    [Header("Flamethrower")]
    [SerializeField] private bool isFlamethrower = false;
    [SerializeField] private float overheatTime = 5f; //time before cant use
    [SerializeField] private float cooldownTime = 3f; //cooldown to reuse
    public bool IsFlamethrower => isFlamethrower;
    public float OverheatTime => overheatTime;
    public float CooldownTime => cooldownTime;


    [Header("Offsets")]
    [SerializeField] private Vector3 weaponPositionOffset;
    [SerializeField] private Vector3 weaponRotationOffset;
    public Vector3 WeaponPositionOffset => weaponPositionOffset;
    public Vector3 WeaponRotationOffset => weaponRotationOffset;

    [Header("Prefabs")]
    [SerializeField] private GameObject weaponWorldPrefab;
    [SerializeField] private GameObject weaponHeldPrefab;
    public GameObject WeaponWorldPrefab => weaponWorldPrefab;
    public GameObject WeaponHeldPrefab => weaponHeldPrefab;


}
