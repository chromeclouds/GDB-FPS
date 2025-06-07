using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float fireRate;
    public int damage;
    public int maxAmmo;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public float spreadAngle;
    public int bulletsPerShot = 1; //shotgun effect
    public ParticleSystem muzzleFlash;

    public Vector3 weaponPositionOffset;
    public Vector3 weaponRotationOffset;

    public float recoilKickback = 1.0f; //how much kick
    public float recoilRecoverySpeed = 5.0f; //how fast gun returns to zero

    public Vector3 adsPositionOffset;
    public float adsSpeed = 10f;
    public bool hasADS = true;
}
