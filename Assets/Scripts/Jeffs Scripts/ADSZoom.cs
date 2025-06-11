using UnityEngine;

public class ADSZoom : MonoBehaviour
{

    private Camera playerCamera;
    private WeaponFire weaponFire;
    private WeaponData weaponData;

    private float originalFOV;
    private float targetFOV;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = Camera.main;
        weaponFire = GetComponent<WeaponFire>();
        if (weaponFire != null )
        {
            weaponData = weaponFire.weaponData;
        }

        originalFOV = playerCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponData != null && weaponData.HasADS)
        {
            if (Input.GetButton("Fire2"))
            {
                targetFOV = weaponData.ADSFOV;
            }
            else
            {
                targetFOV = originalFOV;
            }

            playerCamera.fieldOfView  = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * weaponData.ADSFOVSpeed);
        }
    
    }
}
