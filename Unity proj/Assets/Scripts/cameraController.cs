using UnityEngine;

public class camedraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertmin, lockVertmax;
    [SerializeField] bool invertY;

    float rotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens* Time.deltaTime;


        //give player options to invert look up/down
        if(invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;
        
        //clamp camera on x axis
        rotX = Mathf.Clamp(rotX, lockVertmin, lockVertmax);

        //rotate camera up down on x axis

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //rotate player on y axis to look left right
        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
