using UnityEngine;

public class WeaponADS : MonoBehaviour
{

    private Vector3 hipPosition;
    private Vector3 adsPosition;
    private float adsSpeed;
    private bool isAiming;

    private WeaponData weaponData;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponData = GetComponent<WeaponFire>().weaponData;
        hipPosition = transform.localPosition;
        adsPosition = weaponData.ADSPositionOffset;
        adsSpeed = weaponData.ADSSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponData.HasADS)
        {
            if (Input.GetButton("Fire2")) //right click
            {
                isAiming = true;
            }
            else
            {
                isAiming = false;
            }

            Vector3 targetPosition = isAiming ? adsPosition : hipPosition;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * adsSpeed);
        }
    }
}
