using UnityEngine;


[CreateAssetMenu(fileName = "NewMeleeWeaponData", menuName = "Weapons/Melee Weapon Data")]

public class MeleeWeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float swingSpeed;
    public Vector3 heldPosition;
    public Vector3 heldRotation;

    public GameObject heldPrefab;
    public GameObject worldPrefab;
}
