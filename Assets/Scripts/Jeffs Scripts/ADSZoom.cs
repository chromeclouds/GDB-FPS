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
        weaponData = GetComponent<WeaponData>();

        originalFOV = playerCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponData != null && weaponData.hasADS)
        {
            if (Input.GetButton("Fire2"))
            {
                targetFOV = weaponData.adsFOV;
            }
            else
            {
                targetFOV = originalFOV;
            }

            playerCamera.fieldOfView  = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * weaponData.adsFOVSpeed);
        }
    
    }
}
