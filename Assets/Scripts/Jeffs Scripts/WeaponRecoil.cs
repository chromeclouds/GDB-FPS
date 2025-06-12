using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float kickbackStrength;
    private float recoverySpeed;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * recoverySpeed);
        
    }

    public void Applyrecoil(float kickback, float recovery)
    {
        //apply
        transform.localPosition -= new Vector3(0, 0, kickback);

        //update recoil settings
        kickbackStrength = kickback;
        recoverySpeed = recovery;
    }
}
