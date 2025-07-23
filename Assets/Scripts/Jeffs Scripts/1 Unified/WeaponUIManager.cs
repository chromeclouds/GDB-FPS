using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUIManager : MonoBehaviour
{
    public static WeaponUIManager instance;

    [Header("UI References")]
    public TMP_Text gunNameText;
    public Image gunIcon;
    public TMP_Text ammoTypeText;
    public TMP_Text ammoCountText;
    [Header("Ammo Colors")]
    public Color lightColor = Color.yellow;
    public Color mediumColor = Color.cyan;
    public Color heavyColor = new Color(0.6f, 0.3f, 1f); //should be purp

    private void Awake()
    {
        instance = this;
        HideWeaponUI();

    }

    public void UpdateWeaponUI(WeaponData weaponData, int currentAmmo, int totalAmmo)
    {
        gunNameText.text = weaponData.WeaponName;

        if (weaponData.WeaponIcon != null)
        {
            gunIcon.sprite = weaponData.WeaponIcon;
            gunIcon.enabled = true;
        }
        else
        {
            gunIcon.sprite = null;
            gunIcon.enabled = false;
        }

        ammoTypeText.text = weaponData.AmmotType.ToString();

        ammoCountText.text = $"{currentAmmo} / {totalAmmo}";

        switch (weaponData.AmmotType)
        {
            case AmmoType.Light:
                ammoTypeText.color = lightColor;
                break;
            case AmmoType.Medium:
                ammoTypeText.color = mediumColor;
                break;
            case AmmoType.Heavy:
                ammoTypeText.color = heavyColor;
                break;
        }
    }

    public void HideWeaponUI()
    {
        if (gunNameText != null)
            gunNameText.text = "";
        if (gunIcon != null)
        {
            gunIcon.sprite = null;
            gunIcon.enabled = false;
        }
        if (ammoTypeText != null)
            ammoTypeText.text = "";
        if (ammoCountText != null)
            ammoCountText.text = "";
    }

    public void UpdateAmmoCount(int currentAmmo, int totalAmmo)
    {
        ammoCountText.text = $"{currentAmmo} / {totalAmmo}";

    }

}
