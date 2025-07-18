using UnityEngine;

public class WeaponADS : MonoBehaviour
{

    private Vector3 hipPosition;
    private Vector3 adsPosition;
    private float adsSpeed;
    private bool isAiming;

    private WeaponData weaponData;
    private Transform adsAnchor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponData = GetComponent<WeaponFire>().weaponData;
        hipPosition = transform.localPosition;
        adsSpeed = weaponData.ADSSpeed;

        adsAnchor = transform.Find("ADSAnchor");
        if (adsAnchor != null)
        {
            adsPosition = transform.localPosition;
        }
        else
        {
            adsPosition = weaponData.ADSPositionOffset;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!weaponData.HasADS) return;
        
        isAiming = Input.GetButton("Fire2");
        
        Vector3 targetPosition = isAiming ? adsPosition : hipPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * adsSpeed);
        
    }
}
